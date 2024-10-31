using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace ProjectHr.Users;

public class ResetPasswordMailInput
{
    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }
}