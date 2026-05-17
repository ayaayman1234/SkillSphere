using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Data;
using SkillSphere.Models;

namespace SkillSphere.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var favorites = _context.FavoriteSkills
                .Where(x => x.UserId == user!.Id)
                .ToList();

            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int skillId)
        {
            var user = await _userManager.GetUserAsync(User);

            var exists = _context.FavoriteSkills.Any(x =>
                x.SkillPostId == skillId &&
                x.UserId == user!.Id);

            if (!exists)
            {
                _context.FavoriteSkills.Add(new FavoriteSkill
                {
                    SkillPostId = skillId,
                    UserId = user!.Id
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "SkillPosts");
        }
    }
}