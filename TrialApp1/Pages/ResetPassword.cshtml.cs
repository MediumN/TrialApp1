using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class ResetPasswordModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string NewPassword { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    public string SuccessMessage { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsResetComplete { get; set; } = false;

    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Token))
        {
            ErrorMessage = "Invalid or missing reset token.";
        }
    }

    public void OnPost()
    {
        if (string.IsNullOrWhiteSpace(Token))
        {
            ErrorMessage = "Invalid or missing reset token.";
            return;
        }

        if (!ModelState.IsValid)
        {
            return;
        }

        // Simulate updating password in the system
        Console.WriteLine($"[Simulated Password Reset] Token: {Token}, New Password: {NewPassword}");

        SuccessMessage = "Your password has been successfully reset.";
        IsResetComplete = true;
    }
}
