using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class CreateRoleModel
    {
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}