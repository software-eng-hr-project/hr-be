using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;

namespace ProjectHr.Entities;

public class WorkDate: EntityDto
{
    public int WorkScheduleId { get; set; }
    public string Label { get; set; }
    public string StartHour { get; set; }
    public string EndHour { get; set; }
    public int DayOfTheWeek { get; set; }
    
    [ForeignKey(nameof(WorkScheduleId))]
    public WorkSchedule WorkSchedule { get; set; }
}