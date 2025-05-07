using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrialApp1.Pages.UserSupportHub
{
    public class UserSupportHubModel : PageModel
    {
        #region Binding Models

        [BindProperty]
        public FeedbackInputModel FeedbackModel { get; set; }

        [BindProperty]
        public HelpRequestModel HelpModel { get; set; }

        [BindProperty]
        public SuggestionInputModel SuggestionModel { get; set; }

        public List<FAQItem> FAQList { get; set; }

        #endregion

        #region Input Models

        public class FeedbackInputModel
        {
            [Required(ErrorMessage = "Name is required")]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Please select a feedback type")]
            [Display(Name = "Feedback Type")]
            public string FeedbackType { get; set; }

            [Required(ErrorMessage = "Message is required")]
            [MinLength(10, ErrorMessage = "Please provide at least 10 characters")]
            [Display(Name = "Your Feedback")]
            public string Message { get; set; }
        }

        public class HelpRequestModel
        {
            [Required(ErrorMessage = "Name is required")]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Subject is required")]
            [Display(Name = "Subject")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [MinLength(20, ErrorMessage = "Please provide at least 20 characters")]
            [Display(Name = "Problem Description")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Priority is required")]
            [Display(Name = "Priority")]
            public string Priority { get; set; }
        }

        public class SuggestionInputModel
        {
            [Required(ErrorMessage = "Title is required")]
            [Display(Name = "Suggestion Title")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Category is required")]
            [Display(Name = "Category")]
            public string Category { get; set; }

            [Required(ErrorMessage = "Details are required")]
            [MinLength(20, ErrorMessage = "Please provide at least 20 characters")]
            [Display(Name = "Details")]
            public string Details { get; set; }

            [Display(Name = "Submit Anonymously")]
            public bool IsAnonymous { get; set; }

            [Display(Name = "Your Name")]
            public string Name { get; set; }

            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email Address")]
            public string Email { get; set; }
        }

        public class FAQItem
        {
            public int Id { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }
            public string Category { get; set; }
        }

        #endregion

        public void OnGet()
        {
            // Initialize the FAQ list with predefined questions and answers
            FAQList = new List<FAQItem>
            {
                new FAQItem
                {
                    Id = 1,
                    Question = "How do I update my profile information?",
                    Answer = "To update your profile information, log in to your account and click on the 'My Profile' link in the top-right corner. From there, you can edit your personal details, contact information, and notification preferences. Click 'Save Changes' when you're done.",
                    Category = "Account"
                },
                new FAQItem
                {
                    Id = 2,
                    Question = "How do I report a solar controller issue?",
                    Answer = "If you're experiencing issues with your solar controller, please navigate to the 'Help Center' tab and submit a help request. Select 'Normal' or 'Urgent' priority depending on the severity. Include detailed information about the problem, any error codes displayed, and when the issue started. Our technical team will respond based on the priority level selected.",
                    Category = "Technical"
                },
                new FAQItem
                {
                    Id = 3,
                    Question = "What response times should I expect for support requests?",
                    Answer = "Our response times vary based on the priority level you select:<ul><li><strong>Low Priority:</strong> 2-3 business days</li><li><strong>Normal Priority:</strong> 1 business day</li><li><strong>Urgent Priority:</strong> 2-4 hours during business hours (8am-6pm, Monday-Friday)</li></ul>For critical issues outside business hours, please call our emergency support line.",
                    Category = "Support"
                },
                new FAQItem
                {
                    Id = 4,
                    Question = "How can I monitor my system's performance?",
                    Answer = "You can monitor your renewable energy system's performance through our dashboard. Log in to your account and you'll see real-time data on energy production, consumption, and system health. For detailed analysis, visit the 'Reports' section where you can generate daily, weekly, monthly, or custom period reports. You can also set up automated email reports in your notification settings.",
                    Category = "Usage"
                },
                new FAQItem
                {
                    Id = 5,
                    Question = "How do I reset my password?",
                    Answer = "To reset your password, click on the 'Forgot Password' link on the login page. Enter the email address associated with your account, and we'll send you a password reset link. Follow the instructions in the email to create a new password. For security reasons, password reset links expire after 24 hours.",
                    Category = "Account"
                },
                new FAQItem
                {
                    Id = 6,
                    Question = "How can I maximize energy efficiency with my system?",
                    Answer = "To maximize energy efficiency:<ul><li>Regularly clean solar panels (if applicable)</li><li>Use the scheduler feature to run high-energy appliances during peak production hours</li><li>Check the energy usage reports to identify consumption patterns</li><li>Enable power-saving mode during low activity periods</li><li>Consider adding additional capacity if you consistently reach limits</li></ul>Our system will also provide automated recommendations based on your usage patterns.",
                    Category = "Usage"
                },
            };
        }
    }
}
