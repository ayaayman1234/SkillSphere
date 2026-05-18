using Microsoft.AspNetCore.Identity;

namespace SkillSphere.Models
{
    public class SwapRequest
    {
        public int Id { get; set; }

        public string FromUserId { get; set; } = string.Empty;
        public string ToUserId { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        // Navigation Properties
        public ApplicationUser? FromUser { get; set; }
        public ApplicationUser? ToUser { get; set; }
    }
}