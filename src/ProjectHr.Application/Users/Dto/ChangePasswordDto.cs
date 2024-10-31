using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace ProjectHr.Users.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxPasswordLength)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPasswordLength)]
        public string NewPassword { get; set; }
    }
}
