﻿using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using ProjectHr.Entities;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class ProjectWithUserDto: EntityDto
{
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public string Status { get; set; }
    
    [CanBeNull] public string Description { get; set; }
    
    [CanBeNull] public ICollection<ProjectMemberWithUserDto> ProjectMembers {get; set; }
}