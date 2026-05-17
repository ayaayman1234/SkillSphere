using System.ComponentModel.DataAnnotations;

namespace SkillSphere.Models
{
    public class SkillPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Offer { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        public string ImageUrl { get; set; }
            = "/images/hero.png";

        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}