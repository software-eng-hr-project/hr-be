using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using JetBrains.Annotations;
using ProjectHr.Enums;

namespace ProjectHr.Users.Dto;

public class GetUserGeneralInfo : EntityDto<long>
{
    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }
    public string AvatarUrl { get; set; } 

    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    [CanBeNull]
    public string WorkEmailAddress { get; set; } = null;

    [Phone]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    [CanBeNull]
    public string WorkPhone { get; set; } = null;

    [Required] public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

    [Required]
    public DateTime JobStartDate { get; set; }
        
    [Required]
    public int JobTitleId { get; set; }
}