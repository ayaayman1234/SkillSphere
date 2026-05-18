using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Data;
using SkillSphere.Models;


namespace SkillSphere.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        // ================= USERS PAGE =================
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // ================= MAKE ADMIN (AJAX) =================
        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                TempData["success"] = $"{user.Email} became Admin successfully";
            }
            else
            {
                TempData["error"] = "User is already Admin";
            }

            return RedirectToAction("Users");
        }

        // ================= DASHBOARD =================

        public IActionResult Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = _userManager.Users.Count(),
                TotalSkills = _context.SkillPosts.Count(),
                TotalRequests = _context.SwapRequests.Count(),
                TotalMessages = _context.Messages.Count()
            };

            return View(model);
        }

        // ================= DELETE USER =================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                TempData["success"] = "User deleted successfully";
            else
                TempData["error"] = "Delete failed";

            return RedirectToAction("Users");
    }
    }
}