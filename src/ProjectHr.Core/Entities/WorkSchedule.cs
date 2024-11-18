using System.Collections;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace ProjectHr.Entities;

public class WorkSchedule: EntityDto
{
    public string Name { get; set; }
    public ICollection<WorkDate> Dates { get; set; }
}