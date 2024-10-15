using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;

namespace ProjectHr.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly ProjectHrDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(ProjectHrDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void SetRole(string roleName, int roleId)
        {
            var permissions = PermissionNames.GetRolePermissions(roleName);
        
            foreach (var permissionName in permissions)
            {
                var p = new RolePermissionSetting
                {
                    TenantId = _tenantId,
                    Name = permissionName,
                    IsGranted = true,
                    RoleId = roleId
                };
                _context.Permissions.Add(p);
            }
        }
        private void CreateRolesAndUsers()
        {
            // Admin role
            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            
            // Manager role
            var managerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Manager);
            if (managerRole == null)
            {
                managerRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Manager, StaticRoleNames.Tenants.Manager) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            
            // Employee role
            var employeeRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Employee);
            if (employeeRole == null)
            {
                employeeRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Employee, StaticRoleNames.Tenants.Employee) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            SetRole(StaticRoleNames.Tenants.Admin, adminRole.Id );
            SetRole(StaticRoleNames.Tenants.Manager, managerRole.Id);
            SetRole(StaticRoleNames.Tenants.Employee, employeeRole.Id);
            // Grant all permissions to admin role

            // var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
            //     .OfType<RolePermissionSetting>()
            //     .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
            //     .Select(p => p.Name)
            //     .ToList();
            //
            // var permissions = PermissionFinder
            //     .GetAllPermissions(new ProjectHrAuthorizationProvider())
            //     .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
            //                 !grantedPermissions.Contains(p.Name))
            //     .ToList();
            //
            // if (permissions.Any())
            // {
            //     _context.Permissions.AddRange(
            //         permissions.Select(permission => new RolePermissionSetting
            //         {
            //             TenantId = _tenantId,
            //             Name = permission.Name,
            //             IsGranted = true,
            //             RoleId = adminRole.Id
            //         })
            //     );
            //     _context.SaveChanges();
            // }
            
            

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }
        }
    }
}
