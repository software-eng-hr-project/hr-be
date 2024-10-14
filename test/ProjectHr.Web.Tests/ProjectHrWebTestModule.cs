using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectHr.EntityFrameworkCore;
using ProjectHr.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ProjectHr.Web.Tests
{
    [DependsOn(
        typeof(ProjectHrWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ProjectHrWebTestModule : AbpModule
    {
        public ProjectHrWebTestModule(ProjectHrEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectHrWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ProjectHrWebMvcModule).Assembly);
        }
    }
}