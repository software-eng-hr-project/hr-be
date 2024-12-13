using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using JetBrains.Annotations;
using ProjectHr.Authorization.Users;
using ProjectHr.Constants;
using ProjectHr.DataAccess.Dto;
using ProjectHr.Enums;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Users.Dto;

[AutoMapFrom(typeof(User))]
public class ProjectUserDto: EntityDto<long>
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

    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;


    public DateTime JobStartDate { get; set; }
        
    [Required]
    public JobTitleDto JobTitle { get; set; }
    
    public string Country { get; set; }
    public string City { get; set; }
    
    public bool IsActive { get; set; }

}