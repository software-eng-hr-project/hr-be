using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Bugsnag.Payload;
using JetBrains.Annotations;

namespace ProjectHr.Entities;

public class EmployeeLayoffInfo: FullAuditedEntity
{
    public DateTime DismissalDate { get; set; }
    public int EmployeeLayoffId { get; set; }
    
    [CanBeNull] public string LayoffReason { get; set; }
    
    [ForeignKey(nameof(EmployeeLayoffId))]
    public EmployeeLayoff EmployeeLayoff { get; set; }
    
    // [CanBeNull] public User User {get; set; }
}