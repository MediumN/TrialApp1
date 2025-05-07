using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ForgotPasswordModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    public string SuccessMessage { get; set; }
    public string ErrorMessage { get; set; }

    // Mock list of registered users
    private static readonly List<string> RegisteredEmails = new()
    {
        "user1@example.com",
        "admin@renewableapp.com",
        "technician@solargrid.co.za"
    };

    public void OnGet()
    {
    }

    public void OnPost()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (RegisteredEmails.Contains(Email.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            // Simulate token generation
            var token = Guid.NewGuid().ToString();
            var resetLink = Url.Page("/ResetPassword", null, new { token }, Request.Scheme);

            // Simulate sending email
            Console.WriteLine($"[Simulated Email] Reset link for {Email}: {resetLink}");

            SuccessMessage = "A password reset link has been sent to your email address.";
        }
        else
        {
            ErrorMessage = "Email address not registered.";
        }
    }
}
