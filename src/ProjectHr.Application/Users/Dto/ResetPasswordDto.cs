using System.ComponentModel.DataAnnotations;

namespace ProjectHr.Users.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }
        
        [Required]
        public string Token { get; set; }
    }
}
