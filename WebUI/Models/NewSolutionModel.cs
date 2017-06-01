using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class NewSolutionModel
    {

        public int TaskId { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Решение")]
        public string Content { get; set; }
    }
}