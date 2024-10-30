using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ProjectHr.Authorization
{
    public class ProjectHrAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Create_User, L("CreateUser"));
            context.CreatePermission(PermissionNames.List_User, L("ListUser"));
            context.CreatePermission(PermissionNames.View_Info_User, L("ViewInfoUser"));
            context.CreatePermission(PermissionNames.Update_Info_User, L("UpdateInfoUser"));
            context.CreatePermission(PermissionNames.ActiveOrDisabled_User, L("ActiveOrDisabledUser"));
            context.CreatePermission(PermissionNames.Delete_User, L("DeleteUser"));
            context.CreatePermission(PermissionNames.Create_Project, L("CreateProject"));
            context.CreatePermission(PermissionNames.Update_Project, L("UpdateProject"));
            context.CreatePermission(PermissionNames.Delete_Project, L("DeleteProject"));
            context.CreatePermission(PermissionNames.List_Project, L("ListProject"));
            context.CreatePermission(PermissionNames.Edit_User_Time_Vacation, L("EditUserTimeVacation"));
            context.CreatePermission(PermissionNames.Approval_Or_Reject_Vacation, L("ApprovalOrRejectVacation"));
            context.CreatePermission(PermissionNames.List_Vacation, L("ListVacation"));
            context.CreatePermission(PermissionNames.Create_Role, L("CreateRole"));
            context.CreatePermission(PermissionNames.Update_Role, L("UpdateRole"));
            context.CreatePermission(PermissionNames.Delete_Role, L("DeleteRole"));
            context.CreatePermission(PermissionNames.List_Role, L("ListRole"));
            context.CreatePermission(PermissionNames.Edit_Authorization, L("EditAuthorization"));
            context.CreatePermission(PermissionNames.Delete_Authorization, L("DeleteAuthorization"));
            context.CreatePermission(PermissionNames.List_Authorization, L("ListAuthorization"));
            context.CreatePermission(PermissionNames.Page_Report, L("PageReport"));
            context.CreatePermission(PermissionNames.List_Settings, L("ListSettings"));
            context.CreatePermission(PermissionNames.List_Employee_Layoff, L("ListEmployeeLayoff"));
            context.CreatePermission(PermissionNames.Create_Vacation_Approval_Process,L("CreateVacationApprovalProcess"));
            context.CreatePermission(PermissionNames.Update_Vacation_Approval_Process,L("UpdateVacationApprovalProcess"));
            context.CreatePermission(PermissionNames.Delete_Vacation_Approval_Process,L("DeleteVacationApprovalProcess"));
            context.CreatePermission(PermissionNames.List_Vacation_Approval_Process, L("ListVacationApprovalProcess"));
            context.CreatePermission(PermissionNames.Create_Working_Schedule, L("CreateWorkingSchedule"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"),
                multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectHrConsts.LocalizationSourceName);
        }
    }
}