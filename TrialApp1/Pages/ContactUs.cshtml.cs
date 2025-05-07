using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrialApp1.Pages
{
    public class ContactUsModel : PageModel
    {
        private readonly ILogger<ContactUsModel> _logger;

        // Static list to simulate storing messages (would be replaced with database in production)
        private static readonly List<ContactForm> ReceivedMessages = new List<ContactForm>();

        public ContactUsModel(ILogger<ContactUsModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public ContactForm Contact { get; set; }

        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // Initialize the contact form
            Contact = new ContactForm();
        }

        public IActionResult OnPost()
        {
            // Check for honeypot field to prevent spam submissions
            if (!string.IsNullOrEmpty(Contact.HoneypotField))
            {
                // Likely a bot filling out hidden fields - reject submission but don't tell the user why
                ModelState.Clear();
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Log the contact submission
                _logger.LogInformation($"New contact form submission from {Contact.FullName} ({Contact.Email})");

                // Add to our simulated database
                Contact.SubmissionDate = DateTime.Now;
                ReceivedMessages.Add(Contact);

                // In a real application, you would:
                // 1. Save to a database
                // 2. Send notification email to staff
                // 3. Send confirmation email to the submitter

                // Set success message and reset form
                SuccessMessage = "Your message has been sent successfully. We'll get back to you soon!";
                Contact = new ContactForm();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contact form submission");
                ModelState.AddModelError(string.Empty, "An error occurred while sending your message. Please try again later.");
                return Page();
            }
        }

        public class ContactForm
        {
            [Required(ErrorMessage = "Please enter your full name")]
            [Display(Name = "Full Name")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
            public string FullName { get; set; }

            [Required(ErrorMessage = "Please enter your email address")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address")]
            [Display(Name = "Email Address")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                ErrorMessage = "Please enter a valid email address")]
            public string Email { get; set; }

            [Display(Name = "Phone Number")]
            [Phone(ErrorMessage = "Please enter a valid phone number")]
            [RegularExpression(@"^\+?[0-9\s\-\(\)]+$",
                ErrorMessage = "Please enter a valid phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Please select a subject")]
            [Display(Name = "Subject")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "Please enter your message")]
            [Display(Name = "Message")]
            [StringLength(2000, MinimumLength = 10,
                ErrorMessage = "Message must be between 10 and 2000 characters")]
            public string Message { get; set; }

            // Anti-spam honeypot field - should remain empty
            [Display(Name = "Leave this empty")]
            public string HoneypotField { get; set; }

            [Required(ErrorMessage = "Please confirm you're not a robot")]
            [Display(Name = "I'm not a robot")]
            [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm you're not a robot")]
            public bool NotRobot { get; set; }

            // Timestamp for when the message was submitted
            public DateTime SubmissionDate { get; set; }
        }
    }
}