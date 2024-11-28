using AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Entities;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class ProjectMapProfile: Profile
{
    public ProjectMapProfile()
    {
        CreateMap<CreateProjectDto, Project>().ReverseMap();
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<User, ProjectMember>();
        CreateMap<ProjectMember, ProjectMemberDto>();
        CreateMap<ProjectMember, ProjectMemberWithUserDto>();
        CreateMap<ProjectMember, ProjectMemberWithProject>();
        CreateMap<CreateProjectDetailsDto, Project>();
        CreateMap<CreateProjectMemberDto, ProjectMember>();
        CreateMap<UpdateProjectDto, Project>();

    }
}