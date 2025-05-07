using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TrialApp1.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; }

        [TempData]
        public string? ReturnUrl { get; set; }

        public string? ErrorMessage { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/Home");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Mocked login logic
            if ((Input.EmailOrUsername == "admin" || Input.EmailOrUsername == "admin@domain.co.za") &&
                Input.Password == "password123")
            {
                // In production, replace this with actual sign-in and authentication logic
                // Example: HttpContext.SignInAsync(...)

                return LocalRedirect(ReturnUrl ?? "/Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your details.");
            return Page();
        }

        public class LoginInputModel
        {
            [Required(ErrorMessage = "Email or username is required")]
            [Display(Name = "Email or Username")]
            public string EmailOrUsername { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember Me")]
            public bool RememberMe { get; set; }
        }
    }
}
