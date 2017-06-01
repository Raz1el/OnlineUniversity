using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationRole : IdentityRole
    {
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}