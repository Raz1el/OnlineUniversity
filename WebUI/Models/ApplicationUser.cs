using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public ICollection<Classroom> Classroom { get; set; }
    }
}