using System.Collections.Generic;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using ProjectHr.Enums;

namespace ProjectHr.Entities;

public class DayOffType: EntityDto
{
    public string Name { get; set; }

    [CanBeNull] public string Description { get; set; }

    public int MaxAllowedDaysCount { get; set; }

    public bool IsPaid { get; set; }
    
    [CanBeNull] public Gender SpecificGender { get; set; }
    
    [CanBeNull] public ICollection<DayOffRequest> DayOffRequests { get; set; }
}