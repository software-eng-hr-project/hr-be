using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using ProjectHr.Authorization;
using ProjectHr.Entities;
using ProjectHr.Projects.Dto;

namespace ProjectHr.Projects;
[AbpAuthorize]
[Route("/api/project")]
public class ProjectAppService: ProjectHrAppServiceBase
{
    private readonly IRepository<Project> _projectRepository;

    public ProjectAppService(IRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }
    
    [AbpAuthorize(PermissionNames.Create_Project)]
    [HttpPost]
    public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
    {
        var project = ObjectMapper.Map<Project>(input);
        await _projectRepository.UpdateAsync(project);
        await CurrentUnitOfWork.SaveChangesAsync();
        var projectDto = ObjectMapper.Map<ProjectDto>(project);
        return projectDto;
    }
    
}