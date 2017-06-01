using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class Solution
    {
        public int Id { get; set; }
        public ApplicationUser SolutionCreator { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Решение")]
        public string Content { get; set; }
    }
}
