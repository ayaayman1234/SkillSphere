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

            ViewBag.UserSkills = await _context.SkillPosts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var ratings = await _context.Ratings
                .Where(r => r.RatedUserId == userId)
                .Include(r => r.Rater)
                .ToListAsync();

            ViewBag.Ratings = ratings;
            ViewBag.AverageRating = ratings.Any()
                ? Math.Round(ratings.Average(r => r.Score), 1)
                : 0.0;
            ViewBag.RatingCount = ratings.Count;
            ViewBag.AlreadyRated = ratings.Any(r => r.RaterId == currentUserId);
            ViewBag.IsOwner = currentUserId == userId;
            ViewBag.ProfileUserId = userId;

            return View(user);
        }

        // ================= EDIT =================

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            string? fullName,
            string? bio,
            string? learningSkill,
            string? teachingSkill,
            string? availableHours,
            string? contactNumber,
            string? facebookLink,
            string? linkedInLink)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return NotFound();

            user.FullName = fullName;
            user.Bio = bio;
            user.LearningSkill = learningSkill;
            user.TeachingSkill = teachingSkill;
            user.AvailableHours = availableHours;
            user.ContactNumber = contactNumber;
            user.FacebookLink = facebookLink;
            user.LinkedInLink = linkedInLink;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully ✔";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(user);
        }
    }
}