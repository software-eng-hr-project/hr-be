using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ProjectHr.Controllers
{
    public abstract class ProjectHrControllerBase: AbpController
    {
        protected ProjectHrControllerBase()
        {
            LocalizationSourceName = ProjectHrConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
