using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class EditRoleModel
    {
        public string Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}