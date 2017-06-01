using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class NewTaskModel
    {
        [Display(Name = "Название")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Условие задачи")]
        public string Content { get; set; }
        public int ClassroomId { get; set; }
    }
}