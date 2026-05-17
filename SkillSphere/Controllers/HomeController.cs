using Microsoft.AspNetCore.Mvc;
using SkillSphere.Data;
using SkillSphere.Models;
using System.Diagnostics;

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
        public IActionResult ContactUs(
            string name,
            string email,
            string message)
        {
            TempData["Message"] =
                "Message sent successfully ✔";

            return RedirectToAction("ContactUs");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]

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