using System.ComponentModel.DataAnnotations;

namespace SkillSphere.Models
{
    public class Rating
    {
        public int Id { get; set; }

        // اللي عمل الـ Rating
        public string RaterId { get; set; } = string.Empty;
        public ApplicationUser? Rater { get; set; }

        // اللي اتعمله الـ Rating
        public string RatedUserId { get; set; } = string.Empty;
        public ApplicationUser? RatedUser { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        [StringLength(300)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}