using Microsoft.AspNetCore.Identity;

namespace SkillSphere.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? Bio { get; set; }

        public string? LearningSkill { get; set; }

        public string? TeachingSkill { get; set; }

        public string? AvailableHours { get; set; }

        public string? ContactNumber { get; set; }

        public string? FacebookLink { get; set; }

        public string? LinkedInLink { get; set; }
    }
}