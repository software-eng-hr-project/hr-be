using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using ProjectHr.Entities;
using ProjectHr.Enums;

namespace ProjectHr.Projects.Dto;

public class CreateProjectDto
{   
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public ProjectStatus Status { get; set; }
    
    [CanBeNull] public string Description { get; set; }
    
}