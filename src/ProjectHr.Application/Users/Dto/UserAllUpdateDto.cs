using System;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using ProjectHr.Authorization.Users;
using ProjectHr.Enums;
using ProjectHr.JobTitles.Dto;

namespace ProjectHr.Users.Dto;

public class UserAllUpdateDto
{
    
    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }
    [Required]
    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
    [Required]
    public DateTime JobStartDate { get; set; }
    
    [Required]
    public int JobTitleId { get; set; }
    
    public string AvatarUrl { get; set; } 
    
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string WorkEmailAddress { get; set; }
        
    [Phone]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    public string WorkPhone { get; set; }

    public string[] RoleNames { get; set; }
    
    public void Normalize()
    {
        if (RoleNames == null)
        {
            RoleNames = new string[0];
        }
    }
}