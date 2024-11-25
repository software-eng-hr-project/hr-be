using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Constants;
using ProjectHr.Entities;
using ProjectHr.Enums;
using ProjectHr.DataAccess.Dto;

namespace ProjectHr.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        

        public bool IsActive { get; set; }

        public string FullName { get; set; }
        
        public DateTime CreationTime { get; set; }
        
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string WorkEmailAddress { get; set; }
        
        [Phone]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string WorkPhone { get; set; }
        
        [Required]
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
        
        [Required]
        public DateTime JobStartDate { get; set; }
        
        public JobTitleDto JobTitle { get; set; }

        public string[] RoleNames { get; set; }
        
        //additional
        public string AvatarUrl { get; set; } 
        
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
        [StringLength(LengthConstants.MaxDirectionLength)]
        public string AddressDirection { get; set; }
        
        [StringLength(AbpUserBase.MaxNameLength)]
        public string EmergencyContactName { get; set; }
        public string EmergencyContactDegree { get; set; }
        [Phone]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string EmergencyContactPhone { get; set; }
    }
}
