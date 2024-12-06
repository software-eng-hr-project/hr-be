using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using ProjectHr.Constants;
using ProjectHr.DataAccess.Dto;

namespace ProjectHr.Users.Dto;

public class EmployeeLayoffInfoWithLayoffNameDto : EntityDto
{
    public DateTime DismissalDate { get; set; }
    public EmployeeLayoffDto EmployeeLayoff { get; set; }
    
    [StringLength(LengthConstants.MaxLayoffReason)]
    [CanBeNull] public string LayoffReason { get; set; }
}