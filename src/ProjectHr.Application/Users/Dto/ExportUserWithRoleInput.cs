using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ProjectHr.ExportFiles;
using ProjectHr.Extensions;

namespace ProjectHr.Users.Dto;

public class ExportUserWithRoleInput: ExportInput
{
    public ExportUserPropTypes[] Columns { get; set; }
}

public enum ExportUserPropTypes
{
    [Description("User.FullName")]
    [AlternateValue("İsim Soyisim")]
    FullName = 1,
    
    [Description("User.EmailAddress")]
    [AlternateValue("Email Adresi")]
    EmailAddress,
    
    [Description("Status")]
    [AlternateValue("Durum")]
    IsActive,
    
    [Description("RoleNames")] 
    [AlternateValue("Rol")]
    RoleNames,

}
