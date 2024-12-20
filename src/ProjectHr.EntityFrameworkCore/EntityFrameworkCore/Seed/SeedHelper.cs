﻿using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using ProjectHr.EntityFrameworkCore.Seed.EmployeeLayoff;
using ProjectHr.EntityFrameworkCore.Seed.Host;
using ProjectHr.EntityFrameworkCore.Seed.JobTitle;
using ProjectHr.EntityFrameworkCore.Seed.TechStack;
using ProjectHr.EntityFrameworkCore.Seed.Tenants;
using ProjectHr.EntityFrameworkCore.Seed.WorkDate;

namespace ProjectHr.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<ProjectHrDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(ProjectHrDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            new JobTitleSeed(context).Create();
            new TechStackSeed(context).Create();
            new EmployeeLayoffSeed(context).Create();
            new WorkScheduleSeed(context).Create();
            // Host seed
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
