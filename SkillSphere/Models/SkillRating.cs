namespace SkillSphere.Models
{
    public class SkillRating
    {
        public int Id { get; set; }

        public int SkillPostId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int Rate { get; set; }
    }
}