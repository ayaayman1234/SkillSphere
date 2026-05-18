using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillSphere.Data;
using SkillSphere.Models;
using System.Security.Claims;

namespace SkillSphere.Controllers
{
    [Authorize]
    public class SkillPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SkillPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= INDEX =================

        public async Task<IActionResult> Index(
            string search,
            string category,
            string type,
            int page = 1)
        {
            int pageSize = 9;

            var skills = _context.SkillPosts
                .AsNoTracking()
                .AsQueryable();

            // ================= SEARCH =================
            if (!string.IsNullOrEmpty(search))
            {
                skills = skills.Where(x =>
                    x.Title.Contains(search) ||
                    x.Description.Contains(search));
            }

            // ================= CATEGORY =================
            if (!string.IsNullOrEmpty(category))
            {
                skills = skills.Where(x => x.Category == category);
            }

            // ================= TYPE =================
            if (!string.IsNullOrEmpty(type))
            {
                skills = skills.Where(x => x.Type == type);
            }

            // ================= COUNT =================
            var totalSkills = await skills.CountAsync();

            // ================= PAGINATION =================
            var result = await skills
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ================= VIEWBAG =================
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalSkills / (double)pageSize);
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentType = type;

            return View(result);
        }

        // ================= DETAILS =================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var skill = await _context.SkillPosts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (skill == null)
                return NotFound();

            return View(skill);
        }

        // ================= CREATE =================

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(SkillPost model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            model.CreatedAt = DateTime.Now;

            _context.SkillPosts.Add(model);
            await _context.SaveChangesAsync();

            TempData["success"] = "Skill created successfully";

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var skill = await _context.SkillPosts.FindAsync(id);

            if (skill == null)
                return NotFound();

            return View(skill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, SkillPost model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var skill = await _context.SkillPosts.FindAsync(id);

            if (skill == null)
                return NotFound();

            skill.Title = model.Title;
            skill.Description = model.Description;
            skill.Offer = model.Offer;
            skill.Category = model.Category;
            skill.Type = model.Type;

            await _context.SaveChangesAsync();

            TempData["success"] = "Skill updated successfully";

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var skill = await _context.SkillPosts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (skill == null)
                return NotFound();

            return View(skill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _context.SkillPosts.FindAsync(id);

            if (skill != null)
            {
                _context.SkillPosts.Remove(skill);
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Skill deleted successfully";

            return RedirectToAction(nameof(Index));
        }

        // ================= AJAX DELETE =================

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var skill = await _context.SkillPosts.FindAsync(id);

            if (skill == null)
                return Json(new { success = false });

            _context.SkillPosts.Remove(skill);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}