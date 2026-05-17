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

            if (!context.SkillPosts.Any())
            {
                context.SkillPosts.AddRange(
                    new SkillPost { Title = "C#", Description = "Basics of C#", Offer = "SQL", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                    new SkillPost { Title = "ASP.NET Core", Description = "MVC basics", Offer = "React", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                    new SkillPost { Title = "JavaScript", Description = "ES6+", Offer = "Node.js", Category = "Programming", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                    new SkillPost { Title = "UI/UX", Description = "Design basics", Offer = "Frontend", Category = "Design", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now },
                    new SkillPost { Title = "Python", Description = "AI basics", Offer = "Math", Category = "AI", Type = "Teaching", UserId = admin.Id, CreatedAt = DateTime.Now }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}