using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUI.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        [Display(Name = "Название класса")]
        public string Name { get; set; }
        [Display(Name = "Преподаватель")]
        public ApplicationUser Teacher { get; set; }
        [InverseProperty("Classrooms")]
        public ICollection<ApplicationUser> Students { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
