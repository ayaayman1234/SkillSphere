using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillSphere.Data;
using SkillSphere.Models;

namespace SkillSphere.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages
                .OrderByDescending(x => x.SentAt)
                .ToList();

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string receiverId, string content)
        {
            var user = await _userManager.GetUserAsync(User);

            var message = new Message
            {
                SenderId = user!.Id,
                ReceiverId = receiverId,
                Content = content
            };

            _context.Messages.Add(message);

            _context.Notifications.Add(new Notification
            {
                UserId = receiverId,
                Text = "You received a new message"
            });

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}