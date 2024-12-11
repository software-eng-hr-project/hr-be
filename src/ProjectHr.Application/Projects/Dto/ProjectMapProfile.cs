using System.Linq;
using AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Entities;
using ProjectHr.Extensions;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class ProjectMapProfile : Profile
{
    public ProjectMapProfile()
    {
        CreateMap<CreateProjectDto, Project>().ReverseMap();
        CreateMap<Project, ProjectWithUserDto>();
        CreateMap<Project, CareerPageProjectDto>();
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<User, ProjectMember>();
        CreateMap<ProjectMember, ProjectMemberDto>();
        CreateMap<ProjectMember, ProjectMemberWithUserDto>();
        CreateMap<ProjectMember, ProjectMemberWithProject>();
        CreateMap<ProjectMember, ProjectMemberWithJustNameDto>()
            .ForMember(x => x.FullName, opt => opt.MapFrom(x => x.User.FullName))
            .ForMember(x => x.AvatarUrl, opt => opt.MapFrom(x => x.User.AvatarUrl));
        CreateMap<User, ProjectMemberWithJustNameDto>();
        CreateMap<CreateProjectDetailsDto, Project>();
        CreateMap<CreateProjectMemberDto, ProjectMember>();
        CreateMap<UpdateProjectDto, Project>();
    }
}