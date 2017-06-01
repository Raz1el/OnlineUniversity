using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Display(Name = "Группа")]
        public string Group { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        public ICollection<Classroom> Classrooms { get; set; }
    }
}