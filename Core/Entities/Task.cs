namespace Core.Entities
{
    public class Task<TUniversityUser>
    {

        public string Id { get; set; }
        public TUniversityUser TaskCreator { get; set; }
        public string Content { get; set; }
    }
}
