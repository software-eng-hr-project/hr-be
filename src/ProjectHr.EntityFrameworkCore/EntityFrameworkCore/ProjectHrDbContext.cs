using System;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.MultiTenancy;

namespace ProjectHr.EntityFrameworkCore
{
    public class ProjectHrDbContext : AbpZeroDbContext<Tenant, Role, User, ProjectHrDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public ProjectHrDbContext(DbContextOptions<ProjectHrDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
