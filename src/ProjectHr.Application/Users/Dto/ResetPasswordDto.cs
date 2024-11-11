using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace ProjectHr.Users.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxPasswordLength)]
        public string NewPassword { get; set; }
        
        [Required]
        public string Token { get; set; }
    }
}
