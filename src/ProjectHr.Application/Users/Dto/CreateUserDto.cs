using System;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using JetBrains.Annotations;
using ProjectHr.Authorization.Users;
using ProjectHr.Enums;

namespace ProjectHr.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        // [Required]
        // [StringLength(AbpUserBase.MaxUserNameLength)]
        // public string UserName { get; set; }

        // [Required]
        // [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        // [DisableAuditing]
        // public string Password { get; set; }
        
        // public bool IsActive { get; set; }

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
        
        public string[] RoleNames { get; set; }


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

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
