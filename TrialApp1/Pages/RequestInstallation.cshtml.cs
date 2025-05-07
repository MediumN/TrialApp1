using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class RequestInstallationModel : PageModel
{
    [BindProperty]
    public InstallationForm Form { get; set; } = new();

    public List<string> SystemTypes { get; set; } = new() { "Solar Only", "Battery Backup", "Full Hybrid" };

    public bool? SubmissionSuccess { get; set; }

    public void OnGet()
    {
        // Initialize defaults if necessary
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            SubmissionSuccess = false;
            return Page();
        }

        // Simulate saving to database
        await Task.Delay(500); // simulate DB write

        TempData["SuccessMessage"] = "Installation request submitted successfully.";
        SubmissionSuccess = true;

        // Optionally redirect to confirmation page
        // return RedirectToPage("InstallationConfirmation", new { name = Form.FullName });

        return Page(); // Show success on same page
    }

    public class InstallationForm
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string Suburb { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "System Type")]
        public string SystemType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Preferred Installation Date")]
        public DateTime PreferredDate { get; set; }

        [Display(Name = "Additional Comments")]
        public string? AdditionalNotes { get; set; }

        [Required]
        [Display(Name = "Agreement")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms.")]
        public bool AgreeToTerms { get; set; }
    }
}
