using Microsoft.AspNetCore.Identity;
using SkillSphere.Models;

namespace SkillSphere.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@skillsphere.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin",
                    TeachingSkill = "All"
                };

                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // ===== بيضيف بس اللي مش موجود بالعنوان =====
            var existingTitles = context.SkillPosts
                .Select(x => x.Title)
                .ToList();

            var newSkills = new List<SkillPost>
            {
                new SkillPost { Title = "C#", Description = "Basics of C#", Offer = "SQL", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "ASP.NET Core", Description = "MVC basics", Offer = "React", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "JavaScript", Description = "ES6+", Offer = "Node.js", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "UI/UX", Description = "Design basics", Offer = "Frontend", Category = "Design", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Python", Description = "AI basics", Offer = "Math", Category = "AI", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Git & GitHub", Description = "Version control and collaboration workflows", Offer = "CI/CD Basics", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Computer Vision", Description = "Image classification using OpenCV", Offer = "NLP basics", Category = "AI", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Email Marketing", Description = "Building newsletters and automation flows", Offer = "Google Analytics", Category = "Marketing", Type = "Learning", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Blender", Description = "3D modeling and texturing basics", Offer = "UI Design", Category = "Design", Type = "Learning", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Docker", Description = "Containerizing modern applications", Offer = "Kubernetes", Category = "Programming", Type = "Learning", UserId = admin.Id, CreatedAt = DateTime.Now },
                new SkillPost { Title = "Growth Hacking", Description = "Strategies for rapid user acquisition", Offer = "Brand Strategy", Category = "Marketing", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now }
            };

            var toAdd = newSkills
                .Where(s => !existingTitles.Contains(s.Title))
                .ToList();

            if (toAdd.Any())
            {
                context.SkillPosts.AddRange(toAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}