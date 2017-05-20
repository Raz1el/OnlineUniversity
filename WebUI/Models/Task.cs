namespace WebUI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public ApplicationUser TaskCreator { get; set; }
        public string Content { get; set; }
    }
}
