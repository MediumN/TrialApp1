using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class RegisterModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; }

    public string SuccessMessage { get; set; }

    public class InputModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Please enter a valid phone number.")]
        public string MobileNumber { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Required(ErrorMessage = "You must agree to the terms.")]
        [Display(Name = "Agree to Terms")]
        public bool AgreeToTerms { get; set; }
    }

    public void OnGet()
    {
        // Initial load
    }

    public bool ShowSuccessModal { get; set; }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        string hashedPassword = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Input.Password));
        Console.WriteLine($"Registered: {Input.FullName}, {Input.Email}, {Input.Role}, HashedPassword: {hashedPassword}");

        ShowSuccessModal = true; // Trigger modal in Razor
        return Page(); // Re-render with modal
    }
}
