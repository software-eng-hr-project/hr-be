using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using JetBrains.Annotations;
using ProjectHr.DataAccess.Dto;

namespace ProjectHr.ProjectMembers.Dto;

public class ProjectMemberWithJustNameDto : Entity
{
    public JobTitleDto JobTitle { get; set; }
    
    public string AvatarUrl { get; set; } 
    
    public string FullName { get; set; }
    

}