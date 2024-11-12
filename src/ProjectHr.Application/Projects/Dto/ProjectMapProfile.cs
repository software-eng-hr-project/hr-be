using AutoMapper;
using ProjectHr.Entities;

namespace ProjectHr.Projects.Dto;

public class ProjectMapProfile: Profile
{
    public ProjectMapProfile()
    {
        CreateMap<CreateProjectDto, Project>().ReverseMap();
        CreateMap<Project, ProjectDto>();
    }
}