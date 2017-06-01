using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class NewLectureModel
    {
        [Display(Name = "Тема лекции")]
        public string Theme { get; set; }
        [Display(Name = "Краткое описание")]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Текст лекции")]
        public string Content { get; set; }
        public int ClassroomId { get; set; }
    }
}