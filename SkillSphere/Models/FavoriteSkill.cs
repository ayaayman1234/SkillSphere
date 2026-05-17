namespace SkillSphere.Models
{
    public class FavoriteSkill
    {
        public int Id { get; set; }

        public int SkillPostId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public SkillPost? SkillPost { get; set; }
    }
}