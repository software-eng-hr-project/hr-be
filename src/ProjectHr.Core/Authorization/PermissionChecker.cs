using Abp.Authorization;
using ProjectHr.Authorization.Roles;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
