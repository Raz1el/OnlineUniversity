using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}