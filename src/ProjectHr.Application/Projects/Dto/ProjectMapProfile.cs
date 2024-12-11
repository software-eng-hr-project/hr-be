﻿using System.Linq;
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
        CreateMap<Project, ProjectWithUserDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetAlternateValue()));
        CreateMap<Project, CareerPageProjectDto>()
            .ForMember(dest => dest.Manager,
                opt => opt.MapFrom(src => src.ProjectMembers.FirstOrDefault(x => x.IsManager == true).User));
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<User, ProjectMember>();
        CreateMap<ProjectMember, ProjectMemberDto>();
        CreateMap<ProjectMember, ProjectMemberWithUserDto>();
        CreateMap<ProjectMember, ProjectMemberWithProject>();
        CreateMap<ProjectMember, ProjectMemberWithJustNameDto>();
        CreateMap<User, ProjectMemberWithJustNameDto>();
        CreateMap<CreateProjectDetailsDto, Project>();
        CreateMap<CreateProjectMemberDto, ProjectMember>();
        CreateMap<UpdateProjectDto, Project>();
    }
}