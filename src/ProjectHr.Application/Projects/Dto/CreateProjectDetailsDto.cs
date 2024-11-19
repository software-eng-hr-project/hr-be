using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class CreateProjectDetailsDto 
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [CanBeNull] public ICollection<CreateProjectMemberDto> ProjectMembers {get; set; }
}