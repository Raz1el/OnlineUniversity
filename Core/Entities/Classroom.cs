using System.Collections.Generic;

namespace Core.Entities
{
    public class Classroom<TUniversityUser>
    {
        public string Id { get; set; }

        public TUniversityUser Teacher { get; set; }

        public ICollection<TUniversityUser> Students { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Task<TUniversityUser>> Tasks { get; set; }
        public ICollection<Solution<TUniversityUser>> Solutions { get; set; }
    }
}
