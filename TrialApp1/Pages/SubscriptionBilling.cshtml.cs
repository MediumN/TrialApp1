using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcoHub.Pages
{
    public class SubscriptionBillingModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string StatusType { get; set; } // success, danger, warning

        #region Subscription Properties

        [BindProperty]
        public Subscription CurrentSubscription { get; set; }

        [BindProperty]
        public int SelectedPlanId { get; set; }

        public List<SelectListItem> AvailablePlans { get; set; }

        public bool ShowCancellationOptions { get; set; }

        #endregion

        #region Payment Properties

        [BindProperty]
        public PaymentMethod CurrentPaymentMethod { get; set; }

        [BindProperty]
        public PaymentMethod NewPaymentMethod { get; set; }

        #endregion

        #region Billing Properties

        public List<BillingRecord> BillingHistory { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        #endregion

        public async Task<IActionResult> OnGetAsync(int page = 1, DateTime? startDate = null, DateTime? endDate = null)
        {
            // Validate and set pagination parameters
            CurrentPage = page < 1 ? 1 : page;
            StartDate = startDate;
            EndDate = endDate;

            // Load current subscription data
            await LoadSubscriptionDataAsync();

            // Load payment methods
            await LoadPaymentMethodAsync();

            // Load billing history with filtering and pagination
            await LoadBillingHistoryAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateSubscriptionAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSubscriptionDataAsync();
                await LoadPaymentMethodAsync();
                await LoadBillingHistoryAsync();
                StatusMessage = "Please correct the errors and try again.";
                StatusType = "danger";
                return Page();
            }

            try
            {
                // Simulate processing time
                await Task.Delay(1500);

                // In a real application, this would call a service to update the subscription
                // For demo purposes, we just update the local object
                CurrentSubscription.PlanId = SelectedPlanId;
                CurrentSubscription.PlanName = GetPlanNameById(SelectedPlanId);
                CurrentSubscription.RenewalDate = DateTime.Now.AddMonths(1);

                StatusMessage = "Your subscription has been updated successfully!";
                StatusType = "success";
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                StatusType = "danger";
            }

            // Reload subscription data after update
            await LoadSubscriptionDataAsync();
            await LoadPaymentMethodAsync();
            await LoadBillingHistoryAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostCancelSubscriptionAsync()
        {
            try
            {
                // Simulate processing time
                await Task.Delay(1000);

                // In a real application, this would call a service to cancel the subscription
                CurrentSubscription.Status = "Cancelled";
                CurrentSubscription.RenewalDate = null;

                StatusMessage = "Your subscription has been cancelled. It will remain active until the end of your current billing period.";
                StatusType = "warning";
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                StatusType = "danger";
            }

            // Reload data after cancellation
            await LoadSubscriptionDataAsync();
            await LoadPaymentMethodAsync();
            await LoadBillingHistoryAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostUpdatePaymentMethodAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSubscriptionDataAsync();
                await LoadPaymentMethodAsync();
                await LoadBillingHistoryAsync();
                StatusMessage = "Please correct the payment information and try again.";
                StatusType = "danger";
                return Page();
            }

            try
            {
                // Simulate processing time
                await Task.Delay(2000);

                // In a real application, this would securely update payment details
                CurrentPaymentMethod = NewPaymentMethod;
                CurrentPaymentMethod.LastUpdated = DateTime.Now;

                StatusMessage = "Your payment method has been updated successfully!";
                StatusType = "success";
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                StatusType = "danger";
            }

            // Reload data after payment update
            await LoadSubscriptionDataAsync();
            await LoadPaymentMethodAsync();
            await LoadBillingHistoryAsync();

            return Page();
        }

        public IActionResult OnGetDownloadInvoice(string invoiceId)
        {
            // In a real application, this would generate or fetch a PDF invoice
            // For demo purposes, we return a ContentResult to indicate success
            return Content($"Invoice {invoiceId} would be downloaded in a real application.");
        }

        #region Helper Methods

        private async Task LoadSubscriptionDataAsync()
        {
            // In a real application, this would fetch from a database or API
            // For demo purposes, we create mock data
            CurrentSubscription = new Subscription
            {
                Id = 12345,
                UserId = "user-123",
                PlanId = 2,
                PlanName = "Eco Plus",
                Price = 29.99m,
                Status = "Active",
                StartDate = DateTime.Now.AddMonths(-3),
                RenewalDate = DateTime.Now.AddDays(17),
                Features = new List<string> { "Advanced Analytics", "Unlimited Projects", "API Access", "Priority Support" }
            };

            // Populate available plans
            AvailablePlans = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Eco Basic - $9.99/month" },
                new SelectListItem { Value = "2", Text = "Eco Plus - $29.99/month" },
                new SelectListItem { Value = "3", Text = "Eco Pro - $59.99/month" },
                new SelectListItem { Value = "4", Text = "Eco Enterprise - $199.99/month" }
            };

            // Set selected plan
            SelectedPlanId = CurrentSubscription.PlanId;

            // Determine if cancellation options should be shown
            ShowCancellationOptions = CurrentSubscription.Status == "Active";

            await Task.CompletedTask; // To make the method async
        }

        private async Task LoadPaymentMethodAsync()
        {
            // In a real application, this would fetch from a database or API
            // For demo purposes, we create mock data
            CurrentPaymentMethod = new PaymentMethod
            {
                Id = "pm-5678",
                UserId = "user-123",
                Type = "CreditCard",
                CardBrand = "Visa",
                LastFour = "4242",
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                IsDefault = true,
                LastUpdated = DateTime.Now.AddMonths(-2)
            };

            // Initialize new payment method object for the form
            NewPaymentMethod = new PaymentMethod
            {
                UserId = "user-123",
                Type = "CreditCard"
            };

            await Task.CompletedTask; // To make the method async
        }

        private async Task LoadBillingHistoryAsync()
        {
            // In a real application, this would fetch from a database or API with filtering
            // For demo purposes, we create mock data
            var allBillingRecords = new List<BillingRecord>
            {
                new BillingRecord
                {
                    Id = "inv-001",
                    UserId = "user-123",
                    Amount = 29.99m,
                    Date = DateTime.Now.AddDays(-3),
                    Status = "Paid",
                    PaymentMethod = "Visa •••• 4242",
                    Description = "Eco Plus Monthly Subscription"
                },
                new BillingRecord
                {
                    Id = "inv-002",
                    UserId = "user-123",
                    Amount = 29.99m,
                    Date = DateTime.Now.AddMonths(-1),
                    Status = "Paid",
                    PaymentMethod = "Visa •••• 4242",
                    Description = "Eco Plus Monthly Subscription"
                },
                new BillingRecord
                {
                    Id = "inv-003",
                    UserId = "user-123",
                    Amount = 29.99m,
                    Date = DateTime.Now.AddMonths(-2),
                    Status = "Paid",
                    PaymentMethod = "Visa •••• 4242",
                    Description = "Eco Plus Monthly Subscription"
                },
                new BillingRecord
                {
                    Id = "inv-004",
                    UserId = "user-123",
                    Amount = 9.99m,
                    Date = DateTime.Now.AddMonths(-3),
                    Status = "Paid",
                    PaymentMethod = "Visa •••• 4242",
                    Description = "Eco Basic Monthly Subscription"
                },
                new BillingRecord
                {
                    Id = "inv-005",
                    UserId = "user-123",
                    Amount = 9.99m,
                    Date = DateTime.Now.AddMonths(-4),
                    Status = "Paid",
                    PaymentMethod = "Visa •••• 4242",
                    Description = "Eco Basic Monthly Subscription"
                },
                new BillingRecord
                {
                    Id = "inv-006",
                    UserId = "user-123",
                    Amount = 9.99m,
                    Date = DateTime.Now.AddMonths(-5),
                    Status = "Paid",
                    PaymentMethod = "Visa •••• 4242",
                    Description = "Eco Basic Monthly Subscription"
                }
            };

            // Apply date filtering if specified
            var filteredRecords = allBillingRecords;
            if (StartDate.HasValue || EndDate.HasValue)
            {
                filteredRecords = allBillingRecords.Where(r =>
                    (!StartDate.HasValue || r.Date >= StartDate.Value) &&
                    (!EndDate.HasValue || r.Date <= EndDate.Value)
                ).ToList();
            }

            // Calculate pagination
            TotalPages = (int)Math.Ceiling(filteredRecords.Count / (double)PageSize);

            // Apply pagination
            BillingHistory = filteredRecords
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            await Task.CompletedTask; // To make the method async
        }

        private string GetPlanNameById(int planId)
        {
            return planId switch
            {
                1 => "Eco Basic",
                2 => "Eco Plus",
                3 => "Eco Pro",
                4 => "Eco Enterprise",
                _ => "Unknown Plan"
            };
        }

        #endregion
    }

    #region Model Classes

    public class Subscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public List<string> Features { get; set; }
    }

    public class PaymentMethod
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; } // CreditCard, PayPal, BankTransfer

        [Display(Name = "Card Number")]
        [CreditCard]
        [Required(ErrorMessage = "Card number is required")]
        public string CardNumber { get; set; }

        public string CardBrand { get; set; } // Visa, MasterCard, etc.

        [Display(Name = "Last Four Digits")]
        public string LastFour { get; set; }

        [Display(Name = "Cardholder Name")]
        [Required(ErrorMessage = "Cardholder name is required")]
        public string CardholderName { get; set; }

        [Display(Name = "Expiry Month")]
        [Range(1, 12, ErrorMessage = "Please enter a valid month (1-12)")]
        [Required(ErrorMessage = "Expiration month is required")]
        public int ExpiryMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Range(2023, 2035, ErrorMessage = "Please enter a valid year")]
        [Required(ErrorMessage = "Expiration year is required")]
        public int ExpiryYear { get; set; }

        [Display(Name = "CVV")]
        [Required(ErrorMessage = "CVV is required")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3-4 digits")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must contain only digits")]
        public string CVV { get; set; }

        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }

        [Display(Name = "Billing City")]
        public string BillingCity { get; set; }

        [Display(Name = "Billing State/Province")]
        public string BillingState { get; set; }

        [Display(Name = "Billing ZIP/Postal Code")]
        public string BillingZip { get; set; }

        [Display(Name = "Billing Country")]
        public string BillingCountry { get; set; }

        public bool IsDefault { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class BillingRecord
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // Paid, Pending, Failed
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
    }

    #endregion
}