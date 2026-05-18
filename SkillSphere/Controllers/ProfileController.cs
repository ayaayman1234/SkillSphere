using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillSphere.Data;
using SkillSphere.Models;
using System.Security.Claims;

namespace SkillSphere.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string? id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = id ?? currentUserId;

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var skills = await _context.SkillPosts
                .Where(x => x.UserId == userId)
                .ToListAsync();

           
            ViewBag.ProfileUserId = userId;
            ViewBag.IsOwner = currentUserId == userId;

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Bio = model.Bio;
            user.LearningSkill = model.LearningSkill;
            user.TeachingSkill = model.TeachingSkill;
            user.AvailableHours = model.AvailableHours;
            user.ContactNumber = model.ContactNumber;
            user.FacebookLink = model.FacebookLink;
            user.LinkedInLink = model.LinkedInLink;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                TempData["Toast"] = "Profile updated successfully ✔";
            else
                TempData["Toast"] = "Update failed ❌";

            return RedirectToAction(nameof(Index));
        }
    }
}