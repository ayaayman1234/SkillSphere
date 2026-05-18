using Microsoft.AspNetCore.Mvc;
using SkillSphere.Data;
using SkillSphere.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SkillSphere.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Landing");
        }

        // ================= HOME =================

        public IActionResult Landing()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalSkills = _context.SkillPosts.Count();
            ViewBag.TotalExchanges = _context.SwapRequests.Count();

            return View();
        }

        // ================= ABOUT =================

        public IActionResult AboutUs()
        {
            return View();
        }

        // ================= CONTACT =================

        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(
            string name,
            string email,
            string message)
        {
            // ===== حفظ الـ Message =====
            var newMessage = new Message
            {
                SenderId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest",
                ReceiverId = "admin",
                Content = $"From: {name} ({email})\n\n{message}",
                SentAt = DateTime.Now
            };

            _context.Messages.Add(newMessage);

            // ===== إرسال Notification للـ Admin =====
            var admin = _context.Users
                .FirstOrDefault(u => u.Email == "admin@skillsphere.com");

            if (admin != null)
            {
                _context.Notifications.Add(new Notification
                {
                    UserId = admin.Id,
                    Text = $"New contact message from {name}",
                    IsRead = false,
                    CreatedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Message sent successfully ✔";

            return RedirectToAction("ContactUs");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId =
                Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
            });
        }
    }
}