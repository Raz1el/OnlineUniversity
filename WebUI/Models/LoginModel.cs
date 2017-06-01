using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}