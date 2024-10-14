using System.ComponentModel.DataAnnotations;

namespace ProjectHr.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}