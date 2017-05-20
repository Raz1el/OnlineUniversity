namespace WebUI.Models
{
    public class Solution
    {
        public int Id { get; set; }
        public ApplicationUser SolutionCreator { get; set; }
        public Task Task { get; set; }

        public string Content { get; set; }
    }
}
