using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using ProjectHr.Entities;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class CreateProjectDto
{   
    [Required]
    public string Name { get; set; }

    [CanBeNull] public string Description { get; set; }
    
    [Required]
    public ProjectType Type { get; set; }
    
    [Required]
    public CreateProjectManagerDto Manager { get; set; }
    
}