using System;
using System.Collections.Generic;
using ProjectHr.Authorization.Roles;

namespace ProjectHr.Authorization
{
    public static class PermissionNames
    {
        public const string Pages_Tenants = "Pages.Tenants";

        public const string Pages_Users = "Pages.Users";
        public const string Pages_Users_Create = "Pages.Users.Create";
        public const string Pages_Users_Read_All_Infos = "Pages.Users.Read.All.Infos";
        public const string Pages_Users_Update_All_Infos = "Pages.Users.Update.All.Infos";
        public const string Pages_Users_Delete = "Pages.Users.Delete";
        
        
        public const string Pages_Users_Activation = "Pages.Users.Activation";

        public const string Pages_Roles = "Pages.Roles";

    public static List<string> GetRolePermissions(string roleName)
        {
            if (roleName == StaticRoleNames.Tenants.Admin)
            {
                return new List<string>()
                {
                    Pages_Tenants,
                    Pages_Users,
                    Pages_Users_Create,
                    Pages_Users_Read_All_Infos,
                    Pages_Users_Activation,
                    Pages_Roles,
                    Pages_Users_Update_All_Infos,
                    Pages_Users_Delete
                };
            }

            if  (StaticRoleNames.Tenants.Manager == roleName)
            {
                return new List<string>()
                {
                    Pages_Tenants,
                    Pages_Users,
                };
            }

            if (StaticRoleNames.Tenants.Employee == roleName)
            {
                return new List<string>()
                {
                    Pages_Tenants,
                    Pages_Users,
                };
            }

            throw new NotImplementedException();
        }
    }
}
