using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUI.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ApplicationUser Teacher { get; set; }
        [InverseProperty("Classroom")]
        public ICollection<ApplicationUser> UniversityUser { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<Solution> Solutions { get; set; }
    }
}
