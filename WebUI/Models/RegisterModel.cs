using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Группа")]
        [Required]
        public string Group { get; set; }
        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Подтвердите пароль")]
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}