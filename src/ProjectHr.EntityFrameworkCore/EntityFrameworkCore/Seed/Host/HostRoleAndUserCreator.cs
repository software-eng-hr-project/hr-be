using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProjectHr.Enums;

namespace ProjectHr.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly ProjectHrDbContext _context;

        public HostRoleAndUserCreator(ProjectHrDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            // Admin role for host

            var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role for host

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == adminRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new ProjectHrAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRoleForHost.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user for host
            var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "admin",
                    Surname = "admin",
                    EmailAddress = "admin@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    JobTitleId = 1
                };

                user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                user.SetNormalizedNames();

                adminUserForHost = _context.Users.Add(user).Entity;
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                _context.SaveChanges();

                _context.SaveChanges();
            }
            string[] firstNames = { "Ahmet", "Mehmet", "Ali", "Ayşe", "Fatma", "Zeynep", "Mustafa", "Emine", "Hüseyin", "Hatice" };
            string[] surnames = { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Kara", "Öztürk", "Arslan", "Yıldız", "Ergün" };
            
            for (int i = 0; i< 10; i++)
            {
                var seedUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == 1 && u.EmergencyContactPhone == $"+90505555050{i}");
                if (seedUserForHost == null)
                {
                    var user = new User
                    {
                        TenantId = 1,
                        UserName = Guid.NewGuid().ToString(),
                        Name = $"{firstNames.GetValue(i)}",
                        Surname = $"{surnames.GetValue(i)}",
                        EmailAddress = $"{firstNames.GetValue(i)?.ToString()?.ToLower()}_{surnames.GetValue(i)?.ToString()?.ToLower()}@example.com",
                        WorkEmailAddress = $"{firstNames.GetValue(i)?.ToString()?.ToLower()}_{surnames.GetValue(i)?.ToString()?.ToLower()}@example.com",
                        WorkPhone = $"+90505555050{i}",
                        PersonalPhone = $"+90505555050{i}",
                        EmergencyContactPhone = $"+90505555050{i}",
                        EmploymentType = EmploymentType.FullTime,
                        Gender = Gender.Female,
                        Nationality = "türkiye",
                        BloodType = BloodType.ANegative,
                        MarriedStatus = MarriedStatus.Single,
                        DisabilityLevel = DisabilityLevel.None,          

                        IsEmailConfirmed = true,
                        IsActive = true,
                        JobTitleId = i + 1,
                        JobStartDate = DateTime.Now,
                    };

                    user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                    user.SetNormalizedNames();

                    seedUserForHost = _context.Users.Add(user).Entity;
                    _context.SaveChanges();

                    // Assign Admin role to admin user
                    _context.UserRoles.Add(new UserRole(1, seedUserForHost.Id, adminRoleForHost.Id));
                    _context.SaveChanges();

                    _context.SaveChanges();
                }
            }
        }
    }
}
