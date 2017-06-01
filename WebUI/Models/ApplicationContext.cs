using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public ApplicationContext() : base("OnlineUniversityDb") { }

        public static ApplicationContext Create()
        {

            return new ApplicationContext();
        }
    }
}