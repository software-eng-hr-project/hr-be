using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Enums;

namespace ProjectHr.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

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
        
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string WorkEmailAddress { get; set; }
        
        [Phone]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string WorkPhone { get; set; }
        
        [Required]
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }
        
        [Required]
        public DateTime JobStartDate { get; set; }

        public string[] RoleNames { get; set; }
    }
}
