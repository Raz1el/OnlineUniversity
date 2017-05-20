namespace Core.Entities
{
    public class Solution<TUniversityUser>
    {
        public string Id { get; set; }
        public TUniversityUser SolutionCreator { get; set; }
        public Task<TUniversityUser> Task { get; set; }

        public string Content { get; set; }
    }
}
