using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;
using ProjectHr.Constants;

namespace ProjectHr.Users.Dto;

public class EmployeeLayoffInfoDto: EntityDto
{
    public DateTime DismissalDate { get; set; }
    public int EmployeeLayoffId { get; set; }
    
    [StringLength(LengthConstants.MaxLayoffReason)]
    [CanBeNull] public string LayoffReason { get; set; }
}