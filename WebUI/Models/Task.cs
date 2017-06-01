using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WebUI.Models
{
    public class Task
    {
        public int Id { get; set; }
        [Display(Name = "Название задачи")]
        public string Title { get; set; } 
        public ApplicationUser TaskCreator { get; set; }
        public ICollection<Solution> Solutions { get; set; }

        [Display(Name = "Условие задачи")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
