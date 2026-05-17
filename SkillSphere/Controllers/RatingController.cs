using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillSphere.Data;
using SkillSphere.Models;
using System.Security.Claims;

namespace SkillSphere.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Submit Rating =====
        [HttpPost]
        public async Task<IActionResult> Submit(string ratedUserId, int score, string? comment)
        {
            var raterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // معينش تعمل Rate لنفسك
            if (raterId == ratedUserId)
            {
                TempData["Error"] = "You cannot rate yourself.";
                return RedirectToAction("Index", "Profile", new { id = ratedUserId });
            }

            // معينش تعمل Rate لنفس الشخص أكتر من مرة
            var existing = await _context.Ratings
                .FirstOrDefaultAsync(r =>
                    r.RaterId == raterId &&
                    r.RatedUserId == ratedUserId);

            if (existing != null)
            {
                // تحديث الـ Rating الموجود
                existing.Score = score;
                existing.Comment = comment;
                existing.CreatedAt = DateTime.Now;
            }
            else
            {
                var rating = new Rating
                {
                    RaterId = raterId!,
                    RatedUserId = ratedUserId,
                    Score = score,
                    Comment = comment,
                    CreatedAt = DateTime.Now
                };
                _context.Ratings.Add(rating);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Rating submitted successfully ✔";
            return RedirectToAction("Index", "Profile", new { id = ratedUserId });
        }
    }
}