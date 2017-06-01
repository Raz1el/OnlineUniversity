using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        [Display(Name = "Тема лекции")]
        public string Theme { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Содержание")]
        public string Content { get; set; }

        public ApplicationUser LectureCreator { get; set; }
    }
}
