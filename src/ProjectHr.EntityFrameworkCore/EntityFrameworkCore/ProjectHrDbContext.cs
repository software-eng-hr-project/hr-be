﻿using System;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;
using ProjectHr.Configurations;
using ProjectHr.Entities;
using ProjectHr.MultiTenancy;

namespace ProjectHr.EntityFrameworkCore
{
    public class ProjectHrDbContext : AbpZeroDbContext<Tenant, Role, User, ProjectHrDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<JobTitle> JobTitles { get; set; }

        public DbSet<EmployeeLayoffInfo> EmployeeLayoffInfos { get; set; }
        public DbSet<EmployeeLayoff> EmployeeLayoff { get; set; }
        
        public DbSet<TechStack> TechStacks { get; set; }

        public DbSet<Project> Projects { get; set; }
        
        public DbSet<WorkDate> WorkDates { get; set; }
        
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public ProjectHrDbContext(DbContextOptions<ProjectHrDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfigurations());
        }
    }
}
