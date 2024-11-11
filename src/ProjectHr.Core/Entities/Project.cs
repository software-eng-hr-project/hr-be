using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using ProjectHr.Enums;

namespace ProjectHr.Entities;

public class Project: FullAuditedEntity
{
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public ProjectStatus Status { get; set; }
    
    [CanBeNull] public string Description { get; set; }
    
    [CanBeNull] public ICollection<ProjectMember> ProjectMembers {get; set; }
}