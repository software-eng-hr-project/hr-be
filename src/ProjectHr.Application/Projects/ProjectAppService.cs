using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.Common.Errors;
using ProjectHr.Common.Exceptions;
using ProjectHr.Entities;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;
using ProjectHr.Projects.Dto;

namespace ProjectHr.Projects;

[AbpAuthorize]
[Route("/api/project")]
public class ProjectAppService : ProjectHrAppServiceBase
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<User, long> _userRepository;
    private readonly UserManager _userManager;
    public ProjectAppService(
        IRepository<Project> projectRepository,
        IRepository<User, long> userRepository,
        UserManager userManager
    )
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _userManager = userManager;
    }

    [AbpAuthorize(PermissionNames.Create_Project)]
    [HttpPost]
    public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
    {
        var project = ObjectMapper.Map<Project>(input);
        project.ProjectMembers = new List<ProjectMember>();
        await _projectRepository.InsertAsync(project);
        await CurrentUnitOfWork.SaveChangesAsync();
        var user = _userRepository.FirstOrDefault(u => u.Id == input.Manager.UserId);
        if (user is null)
        {
            throw ExceptionHelper.Create(ErrorCode.UserCannotFound);
        }

        var member = new ProjectMember();
        member.UserId = user.Id;
        member.ProjectId = project.Id;
        member.IsManager = true;
        member.JobTitleId = 6;
        member.TeamName = "Proje Yöneticisi";
        project.ProjectMembers.Add(member);
        project.Status = ProjectStatus.Taslak;
        await _projectRepository.UpdateAsync(project);
        await CurrentUnitOfWork.SaveChangesAsync();
        var projectDto = ObjectMapper.Map<ProjectDto>(project);
        return projectDto;
    }

    // sadece projectmanager düzenleyebilir
    [HttpPut("details/{projectId}")]
    public async Task<ProjectDto> CreateProjectDetailsAsync(int projectId, CreateProjectDetailsDto input)
    {
        var abpSessionUserId = AbpSession.GetUserId();
        var listOfUserId = input.ProjectMembers.Select(p => p.UserId);

        var project = await _projectRepository.GetAll()
            .Include(p => p.ProjectMembers)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null)
            throw new UserFriendlyException("Böyle bir proje Bulunamadı");
        
        bool isManager = project.ProjectMembers.Any(p => p.UserId == abpSessionUserId && p.IsManager == true);
        if (!isManager)
            throw new UserFriendlyException("Bu projede yetkili değilsiniz.");
        
        foreach (var userId in listOfUserId)
        {
            bool isAlreadyExist = project.ProjectMembers.Any(p => p.UserId == userId  && input.ProjectMembers.Any(x=>x.JobTitleId == p.JobTitleId));
            if (isAlreadyExist)
                throw new UserFriendlyException($"Id'si {userId} olan kullanıcı projede aynı rolle zaten kayıtlı");
        }
        
        foreach (var member in input.ProjectMembers)
        {
            bool isExist = _userRepository.GetAll().Any(u => u.Id == member.UserId);
            if (!isExist)
                throw ExceptionHelper.Create(ErrorCode.UserCannotFound);

            var newMember = new ProjectMember();
            newMember.UserId = member.UserId;
            newMember.ProjectId = project.Id;
            newMember.IsManager = false;
            newMember.TeamName = member.TeamName;
            newMember.JobTitleId = member.JobTitleId;
            newMember.IsContributing = true;

            project.ProjectMembers.Add(newMember);
        }
        project.StartDate = input.StartDate;
        project.EndDate = input.EndDate;

        await _projectRepository.UpdateAsync(project);
        await CurrentUnitOfWork.SaveChangesAsync();

        var projectDto = ObjectMapper.Map<ProjectDto>(project);
        return projectDto;
    }

    [AbpAuthorize(PermissionNames.Update_Project)]
    [HttpPut("{projectId}")]
    public async Task<ProjectDto> UpdateProjectAsync(int projectId, UpdateProjectDto input)
    {
        var project = await _projectRepository.GetAll()
            .Include(p => p.ProjectMembers)
            .ThenInclude(p => p.User)
            .ThenInclude(p => p.JobTitle)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        var updatedProject = ObjectMapper.Map(input, project);
        var currentManager = updatedProject.ProjectMembers.FirstOrDefault(p => p.IsManager == true);
        if (currentManager.UserId != input.Manager.UserId)
        {
            updatedProject.ProjectMembers.Remove(currentManager);
            var newManager = await _userRepository.FirstOrDefaultAsync(u => u.Id == input.Manager.UserId);
            var manager = new ProjectMember();
            manager.UserId = newManager.Id;
            manager.ProjectId = project.Id;
            manager.IsManager = true;
            manager.JobTitleId = 6;
            updatedProject.ProjectMembers.Add(manager);
        }

        await _projectRepository.UpdateAsync(updatedProject);
        await CurrentUnitOfWork.SaveChangesAsync();
        var projectDto = ObjectMapper.Map<ProjectDto>(project);
        return projectDto;
    }
    
    [HttpGet]
    public async Task<List<ProjectDto>> GetAllProjectAsync()
    {
        var abpSessionUserId = AbpSession.GetUserId();
        
        var hasPermission = _userManager
            .GetGrantedPermissionsAsync(_userManager.GetUserById(abpSessionUserId)).Result
            .Any(p => p.Name is PermissionNames.List_Project);

        var project = new List<Project>();
        if (hasPermission)
        {
            var allProjects = await _projectRepository.GetAll()
                .OrderBy(p => p.Id)
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm=>pm.JobTitle)
                .Include(p => p.ProjectMembers)
                .ThenInclude(pm=> pm.User)
                .ToListAsync();
            project.AddRange(allProjects);
        }
        else
        {
            var usersProject = await _projectRepository.GetAll()
                .Include(p => p.ProjectMembers)
                .Where(p => p.ProjectMembers.Any(member => member.UserId == abpSessionUserId))
                .ToListAsync();

            project.AddRange(usersProject);
        }

        var projectDto = ObjectMapper.Map<List<ProjectDto>>(project);
        return projectDto;
    }

    // [AbpAuthorize(PermissionNames.List_Project)]
    [HttpGet("{projectId}")]
    public async Task<ProjectDto> GetProjectByIdAsync(int projectId)
    {
        var project = await _projectRepository.GetAll()
            .Include(p => p.ProjectMembers)
            .ThenInclude(pm=>pm.JobTitle)
            .Include(p => p.ProjectMembers)
            .ThenInclude(pm=> pm.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        var projectDto = ObjectMapper.Map<ProjectDto>(project);
        return projectDto;
    }

    [AbpAuthorize(PermissionNames.Delete_Project)]
    [HttpDelete("{projectId}")]
    public async Task DeleteProject(int projectId)
    {
        var project = await _projectRepository.FirstOrDefaultAsync(p => p.Id == projectId);
        _projectRepository.DeleteAsync(project);

        await CurrentUnitOfWork.SaveChangesAsync();
    }
}