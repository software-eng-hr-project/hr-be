using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ProjectHr.Entities;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class ProjectDto
{
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public ProjectStatus Status { get; set; }
    
    [CanBeNull] public string Description { get; set; }
    
    [CanBeNull] public ICollection<ProjectMemberDto> ProjectMembers {get; set; }
}