using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class EditUserModel
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }
        public  string Email { get; set; }
        [Display(Name = "Группа")]
        public string Group { get; set; }
    }
}