using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ProjectHr.Authorization
{
    public class ProjectHrAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Create, L("UsersCreate"));
            context.CreatePermission(PermissionNames.Pages_Users_Read_All_Infos, L("UsersReadAllInfos"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectHrConsts.LocalizationSourceName);
        }
    }
}
