using System;
using System.Collections.Generic;
using ProjectHr.Authorization.Roles;

namespace ProjectHr.Authorization
{
    public static class PermissionNames
    {
        public const string Pages_Tenants = "Pages.Tenants";
        
        public const string Create_User = "Create.User";
        public const string List_User = "List.User";
        public const string View_Info_User = "View.Info.User";
        public const string Update_Info_User = "Update.Info.User";
        public const string ActiveOrDisabled_User = "ActiveOrDisabled.User";
        public const string Delete_User = "Delete.User";

        public const string Create_Project = "Create.Project";
        public const string Update_Project = "Update.Project";
        public const string Delete_Project = "Delete.Project";
        public const string List_Project = "List.Project";
        public const string Edit_User_Time_Vacation = "Edit.User.TimeVacation";
        public const string Approval_Or_Reject_Vacation = "ApprovalOrReject.Vacation";
        public const string List_Vacation = "List.Vacation";
        public const string Create_Role = "Create.Role";
        public const string Update_Role = "Update.Role";
        public const string Delete_Role = "Delete.Role";
        public const string List_Role = "List.Role";
        public const string Edit_Authorization = "Edit.Authorization";
        public const string Delete_Authorization = "Delete.Authorization";
        public const string List_Authorization = "List.Authorization";
        public const string Page_Report = "Page.Report";
        public const string List_Settings = "List.Settings";
        public const string List_Employee_Layoff = "List.EmployeeLayoff";
        public const string Create_Vacation_Approval_Process = "Create.VacationApprovalProcess";
        public const string Update_Vacation_Approval_Process = "Update.VacationApprovalProcess";
        public const string Delete_Vacation_Approval_Process = "Delete.VacationApprovalProcess";
        public const string List_Vacation_Approval_Process = "List.VacationApprovalProcess";
        public const string Create_Working_Schedule = "Create.WorkingSchedule";
        
        // public const string Pages_Roles = "Pages.Roles";

    public static List<string> GetRolePermissions(string roleName)
        {
            if (roleName == StaticRoleNames.Tenants.Admin)
            {
                return new List<string>()
                {
                    // Pages_Tenants,
                    Create_User,
                    List_User,
                    View_Info_User,
                    Update_Info_User,
                    ActiveOrDisabled_User,
                    Delete_User,
                    Create_Project,
                    Update_Project,
                    Delete_Project,
                    List_Project,
                    Edit_User_Time_Vacation,
                    Approval_Or_Reject_Vacation,
                    List_Vacation,
                    Create_Role,
                    Update_Role,
                    Delete_Role,
                    List_Role,
                    Edit_Authorization,
                    Delete_Authorization,
                    List_Authorization,
                    Page_Report,
                    List_Settings,
                    List_Employee_Layoff,
                    Create_Vacation_Approval_Process,
                    Update_Vacation_Approval_Process,
                    Delete_Vacation_Approval_Process,
                    List_Vacation_Approval_Process,
                    Create_Working_Schedule
                };
            }

            if  (StaticRoleNames.Tenants.Manager == roleName)
            {
                return new List<string>()
                {
                    // Pages_Tenants,
                    Update_Project,
                    List_User,
                    List_Project,
                    Approval_Or_Reject_Vacation,
                    List_Vacation
                };
            }

            if (StaticRoleNames.Tenants.Employee == roleName)
            {
                return new List<string>()
                {
                    // Pages_Tenants,
                };
            }

            throw new NotImplementedException();
        }
    }
}
