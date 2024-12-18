using System.Collections.Generic;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class CareerPageProjectDto :EntityDto
{
    public string Name { get; set; }
    
    public ProjectStatus Status { get; set; }
    public ProjectType Type { get; set; }
    
    [CanBeNull] public string Description { get; set; }
    public ProjectMemberWithJustNameDto ProfileOwner { get; set; }
    public ProjectMemberWithJustNameDto Manager {get; set; }
    public ProjectMemberWithJustNameDto TeamLead {get; set; }
}