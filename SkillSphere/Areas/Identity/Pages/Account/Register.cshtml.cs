using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillSphere.Models;

namespace SkillSphere.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string FullName { get; set; } = "";

            [Required]
            public string Email { get; set; } = "";

            [Required]
            public string Bio { get; set; } = "";

            [Required]
            public string LearningSkill { get; set; } = "";

            [Required]
            public string TeachingSkill { get; set; } = "";

            [Required]
            public string AvailableHours { get; set; } = "";

            [Required]
            public string ContactNumber { get; set; } = "";

            public string? FacebookLink { get; set; }

            public string? LinkedInLink { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Required]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } = "";
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName,
                Bio = Input.Bio,
                LearningSkill = Input.LearningSkill,
                TeachingSkill = Input.TeachingSkill,
                AvailableHours = Input.AvailableHours,
                ContactNumber = Input.ContactNumber,
                FacebookLink = Input.FacebookLink,
                LinkedInLink = Input.LinkedInLink
            };

            var result =
                await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "SkillPosts");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return Page();
        }
    }
}