using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using ProjectHr.DataAccess.Dto;

namespace ProjectHr.Users.Dto;

public class UserRolesPageDto
{
    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }


    public bool IsActive { get; set; }

    public string FullName { get; set; }


    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string WorkEmailAddress { get; set; }


    public JobTitleDto JobTitle { get; set; }

    public string[] RoleNames { get; set; }
    
}