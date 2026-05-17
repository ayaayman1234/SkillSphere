using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Data;
using SkillSphere.Models;

namespace SkillSphere.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var requests = _context.SwapRequests.ToList();

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string toUserId)
        {
            var user = await _userManager.GetUserAsync(User);

            _context.SwapRequests.Add(new SwapRequest
            {
                FromUserId = user!.Id,
                ToUserId = toUserId
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}