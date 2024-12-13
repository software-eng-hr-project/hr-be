using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using JetBrains.Annotations;
using ProjectHr.Entities;
using ProjectHr.Enums;

namespace ProjectHr.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                JobTitleId = 1,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
        public int JobTitleId { get; set; }
        public int? EmployeeLayoffInfoId { get; set; }
        public string AvatarUrl { get; set; } 
        public string WorkEmailAddress { get; set; }
        public string WorkPhone { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public DateTime JobStartDate { get; set; }
        
        public Gender Gender { get; set; }
        public MilitaryStatus MilitaryStatus { get; set; }
        public DateTime Birthday { get; set; }
        public string Nationality { get; set; } 
        public string IdentityNumber { get; set; }
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
        public string AddressDirection { get; set; }
        
        public string EmergencyContactName { get; set; }
        public string EmergencyContactDegree { get; set; }
        public string EmergencyContactPhone { get; set; }
        public bool IsInvited { get; set; } = false;

        [ForeignKey(nameof(JobTitleId))]
        public JobTitle JobTitle { get; set; }
        
        [ForeignKey(nameof(EmployeeLayoffInfoId))]
        public EmployeeLayoffInfo EmployeeLayoffInfo { get; set; }
        
        [CanBeNull] public ICollection<ProjectMember> ProjectMembers {get; set; }
        
        [CanBeNull] public ICollection<TechStack> TechStack { get; set; }

        public bool IsThemeLight { get; set; } = true;

    }
}
