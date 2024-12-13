using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using JetBrains.Annotations;
using ProjectHr.Enums;
using ProjectHr.DataAccess.Dto;

namespace ProjectHr.Users.Dto;

public class GetUserGeneralInfo : EntityDto<long>
{
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }
    
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }
    public string AvatarUrl { get; set; } 

    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; } 
    
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    [CanBeNull]
    public string WorkEmailAddress { get; set; } = null;

    [Phone]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    [CanBeNull]
    public string WorkPhone { get; set; } = null;

    public string EmploymentType { get; set; }


    public DateTime JobStartDate { get; set; }
        
    [Required]
    public JobTitleDto JobTitle { get; set; }
    
    public string Country { get; set; }
    public string City { get; set; }
    
    public bool IsActive { get; set; }
    
    public string[] RoleNames { get; set; }
    public string[] Projects { get; set; }
}