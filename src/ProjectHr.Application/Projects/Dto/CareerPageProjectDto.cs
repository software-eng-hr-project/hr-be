using System.Collections.Generic;
using JetBrains.Annotations;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class CareerPageProjectDto
{
    public string Name { get; set; }
    
    public string Status { get; set; }
    
    [CanBeNull] public string Description { get; set; }
    
    public ProjectMemberWithJustNameDto Manager {get; set; }
    public ProjectMemberWithJustNameDto TeamLead {get; set; }
}