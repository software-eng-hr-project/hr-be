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
    public int Id { get; set; }
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
    
    public Gender Gender { get; set; }
    public MilitaryStatus MilitaryStatus { get; set; }
    public DateTime Birthday { get; set; }
    public string Nationality { get; set; } 
    public string IdentityNumber { get; set; }
    
    [Phone]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    public string PersonalPhone { get; set; } 
    public BloodType BloodType { get; set; }
        
    public MarriedStatus MarriedStatus { get; set; }
    public bool IsSpouseWorking { get; set; }
    public int ChildrenCount { get; set; }
        
    public DisabilityLevel DisabilityLevel { get; set; }
    public bool IsGraduated { get; set; }
    public EducationStatus HigherEducationStatus { get; set; }
        
    public string Country { get; set; }
    public string City { get; set; }
    [StringLength(User.MaxDirectionLength)]
    public string AddressDirection { get; set; }
        
    [StringLength(AbpUserBase.MaxNameLength)]
    public string EmergencyContactName { get; set; }
    public string EmergencyContactDegree { get; set; }
    [Phone]
    [StringLength(AbpUserBase.MaxPhoneNumberLength)]
    public string EmergencyContactPhone { get; set; }
    
    public string[] RoleNames { get; set; }
    
    public void Normalize()
    {
        if (RoleNames == null)
        {
            RoleNames = new string[0];
        }
    }
}