using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrialApp1.Pages
{
    public class MaintenanceModel : PageModel
    {
        private readonly ILogger<MaintenanceModel> _logger;

        public MaintenanceModel(ILogger<MaintenanceModel> logger)
        {
            _logger = logger;
        }

        public List<MaintenanceItem> MaintenanceItems { get; set; } = new List<MaintenanceItem>();

        [BindProperty]
        public MaintenanceRequest NewMaintenance { get; set; } = new MaintenanceRequest();

        public decimal TotalActualCost => MaintenanceItems
            .Where(i => i.Status == "Completed")
            .Sum(i => i.ActualCost);

        public decimal TotalProjectedCost => MaintenanceItems
            .Where(i => i.Status == "Pending")
            .Sum(i => i.ProjectedCost);

        public decimal TotalOverdueCost => MaintenanceItems
            .Where(i => i.Status == "Overdue")
            .Sum(i => i.ProjectedCost);

        public int CompletedMaintenanceCount => MaintenanceItems
            .Count(i => i.Status == "Completed");

        public int PendingMaintenanceCount => MaintenanceItems
            .Count(i => i.Status == "Pending");

        public int OverdueMaintenanceCount => MaintenanceItems
            .Count(i => i.Status == "Overdue");

        public int TotalMaintenanceCount => MaintenanceItems.Count;

        public int CompletedPercentage => TotalMaintenanceCount == 0 ? 0 :
            (int)Math.Round((double)CompletedMaintenanceCount / TotalMaintenanceCount * 100);

        public int PendingPercentage => TotalMaintenanceCount == 0 ? 0 :
            (int)Math.Round((double)PendingMaintenanceCount / TotalMaintenanceCount * 100);

        public int OverduePercentage => TotalMaintenanceCount == 0 ? 0 :
            (int)Math.Round((double)OverdueMaintenanceCount / TotalMaintenanceCount * 100);

        public bool HasOverdueItems => OverdueMaintenanceCount > 0;

        public bool HasHighCostItems => MaintenanceItems
            .Any(i => i.Status != "Completed" && i.ProjectedCost > 2500);

        public void OnGet()
        {
            // In a real application, this data would come from a database
            // For this example, we'll use simulated data
            LoadSampleData();
        }

        public IActionResult OnPostRequestMaintenance()
        {
            if (!ModelState.IsValid)
            {
                LoadSampleData();
                return Page();
            }

            // Calculate the projected cost based on South African labor rates and regulations
            decimal projectedCost = CalculateProjectedCost(NewMaintenance);

            // In a real application, you would save this to a database
            // For this example, we'll just simulate success
            _logger.LogInformation($"New maintenance request: {NewMaintenance.Type} scheduled for {NewMaintenance.Date}");

            // Add success message
            TempData["SuccessMessage"] = "Maintenance request submitted successfully. A technician will be assigned shortly.";

            return RedirectToPage();
        }

        public IActionResult OnPostMarkComplete(string id, decimal actualCost, string technicianName, string completionNotes)
        {
            if (string.IsNullOrEmpty(id) || actualCost < 0)
            {
                ModelState.AddModelError("", "Invalid data provided");
                LoadSampleData();
                return Page();
            }

            // In a real application, you would update the database record
            _logger.LogInformation($"Marking maintenance ID {id} as complete with actual cost R{actualCost}");

            // Add success message
            TempData["SuccessMessage"] = $"Maintenance marked as complete by {technicianName}";

            return RedirectToPage();
        }

        private decimal CalculateProjectedCost(MaintenanceRequest request)
        {
            // Base rates in South African Rand (ZAR)
            // These rates comply with South African labor laws including minimum wage
            decimal baseRate = request.Type switch
            {
                "Routine Inspection" => 750.00m,
                "Panel Cleaning" => 1200.00m,
                "Inverter Service" => 1800.00m,
                "Battery Maintenance" => 1500.00m,
                "Wiring Inspection" => 950.00m,
                "Emergency Repair" => 2500.00m,
                _ => 1000.00m
            };

            // Urgency multiplier
            decimal urgencyMultiplier = request.Urgency switch
            {
                "Low" => 1.0m,
                "Medium" => 1.2m,
                "High" => 1.5m,
                "Emergency" => 2.0m,
                _ => 1.0m
            };

            // Calculate projected cost
            decimal projectedCost = baseRate * urgencyMultiplier;

            // Apply VAT (15% in South Africa)
            projectedCost *= 1.15m;

            // Apply warranty discount if applicable
            if (request.IsWarranty)
            {
                projectedCost *= 0.25m; // 75% discount for warranty work
            }

            return Math.Round(projectedCost, 2);
        }

        // First, let's complete the MaintenanceItem class
        public class MaintenanceItem
        {
            public string Id { get; set; }
            public DateTime Date { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public string Status { get; set; } // "Completed", "Pending", "Overdue"
            public string Technician { get; set; }
            public decimal ProjectedCost { get; set; }
            public decimal ActualCost { get; set; }
            public string Notes { get; set; }
            public bool IsWarranty { get; set; }
            public string Urgency { get; set; }
        }

        // Next, let's create the MaintenanceRequest class with validation
        public class MaintenanceRequest
        {
            [Required(ErrorMessage = "Maintenance type is required")]
            [Display(Name = "Maintenance Type")]
            public string Type { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Date is required")]
            [DataType(DataType.Date)]
            [Display(Name = "Scheduled Date")]
            public DateTime Date { get; set; }

            [Required(ErrorMessage = "Urgency level is required")]
            public string Urgency { get; set; }

            [Display(Name = "Under Warranty")]
            public bool IsWarranty { get; set; }

            [Display(Name = "System Location")]
            public string Location { get; set; }

            [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
            public string Notes { get; set; }
        }

        // Complete the LoadSampleData method in the MaintenanceModel class
        private void LoadSampleData()
        {
            // In a real application, this data would come from a database
            MaintenanceItems = new List<MaintenanceItem>
    {
        new MaintenanceItem
        {
            Id = "M001",
            Date = DateTime.Now.AddDays(-60),
            Type = "Routine Inspection",
            Description = "Annual system inspection and performance check",
            Status = "Completed",
            Technician = "Sipho Nkosi",
            ProjectedCost = 862.50m,
            ActualCost = 850.00m,
            Notes = "All systems functioning within normal parameters. Recommended cleaning in 3 months.",
            IsWarranty = false,
            Urgency = "Low"
        },
        new MaintenanceItem
        {
            Id = "M002",
            Date = DateTime.Now.AddDays(-30),
            Type = "Panel Cleaning",
            Description = "Clean solar panels to remove dust and debris",
            Status = "Completed",
            Technician = "Thabo Molefe",
            ProjectedCost = 1380.00m,
            ActualCost = 1450.00m,
            Notes = "Additional cleaning required due to bird droppings on northern array.",
            IsWarranty = false,
            Urgency = "Medium"
        },
        new MaintenanceItem
        {
            Id = "M003",
            Date = DateTime.Now.AddDays(7),
            Type = "Inverter Service",
            Description = "Regular service of main inverter system",
            Status = "Pending",
            Technician = "Unassigned",
            ProjectedCost = 2070.00m,
            ActualCost = 0.00m,
            Notes = "Customer reported occasional system shutdowns",
            IsWarranty = true,
            Urgency = "Medium"
        },
        new MaintenanceItem
        {
            Id = "M004",
            Date = DateTime.Now.AddDays(-5),
            Type = "Battery Maintenance",
            Description = "Check battery health and connections",
            Status = "Overdue",
            Technician = "Unassigned",
            ProjectedCost = 1725.00m,
            ActualCost = 0.00m,
            Notes = "Priority for backup system functionality",
            IsWarranty = false,
            Urgency = "High"
        },
        new MaintenanceItem
        {
            Id = "M005",
            Date = DateTime.Now.AddDays(14),
            Type = "Wiring Inspection",
            Description = "Inspect and test all wiring connections",
            Status = "Pending",
            Technician = "Unassigned",
            ProjectedCost = 1092.50m,
            ActualCost = 0.00m,
            Notes = "Routine maintenance as per safety regulations",
            IsWarranty = false,
            Urgency = "Low"
        },
        new MaintenanceItem
        {
            Id = "M006",
            Date = DateTime.Now.AddDays(-2),
            Type = "Emergency Repair",
            Description = "Repair damaged panels after storm",
            Status = "Overdue",
            Technician = "Unassigned",
            ProjectedCost = 5750.00m,
            ActualCost = 0.00m,
            Notes = "Customer reports 40% reduced output",
            IsWarranty = false,
            Urgency = "Emergency"
        }
    };
        }

        // Now, let's add a method to assign a technician to a maintenance request
        public IActionResult OnPostAssignTechnician(string id, string technicianName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(technicianName))
            {
                ModelState.AddModelError("", "Both maintenance ID and technician name are required");
                LoadSampleData();
                return Page();
            }

            // In a real application, you would update the database record
            _logger.LogInformation($"Assigning technician {technicianName} to maintenance ID {id}");

            // Add success message
            TempData["SuccessMessage"] = $"Technician {technicianName} assigned successfully to maintenance request {id}";

            return RedirectToPage();
        }

        // Method to update maintenance status
        public IActionResult OnPostUpdateStatus(string id, string status)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("", "Both maintenance ID and status are required");
                LoadSampleData();
                return Page();
            }

            // Validate status
            if (status != "Pending" && status != "Completed" && status != "Overdue")
            {
                ModelState.AddModelError("", "Invalid status value");
                LoadSampleData();
                return Page();
            }

            // In a real application, you would update the database record
            _logger.LogInformation($"Updating maintenance ID {id} status to {status}");

            // Add success message
            TempData["SuccessMessage"] = $"Maintenance status updated successfully to {status}";

            return RedirectToPage();
        }

        // Method to generate a maintenance report
        public IActionResult OnPostGenerateReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                ModelState.AddModelError("", "Start date must be before end date");
                LoadSampleData();
                return Page();
            }

            // In a real application, you would query the database for this date range
            // and possibly generate a PDF or Excel report
            _logger.LogInformation($"Generating maintenance report from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            // For this example, we'll just set a message
            TempData["SuccessMessage"] = $"Report generated for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}";

            LoadSampleData();
            return Page();
        }

        // Utility method to get maintenance items by status
        public List<MaintenanceItem> GetMaintenanceItemsByStatus(string status)
        {
            return MaintenanceItems.Where(i => i.Status == status).ToList();
        }

        // Method to calculate maintenance efficiency
        public double CalculateMaintenanceEfficiency()
        {
            if (CompletedMaintenanceCount == 0)
            {
                return 0;
            }

            decimal totalProjected = MaintenanceItems
                .Where(i => i.Status == "Completed")
                .Sum(i => i.ProjectedCost);

            decimal totalActual = MaintenanceItems
                .Where(i => i.Status == "Completed")
                .Sum(i => i.ActualCost);

            if (totalProjected == 0)
            {
                return 0;
            }

            return (double)(totalProjected / totalActual * 100);
        }

        // Method to get maintenance cost by type
        public Dictionary<string, decimal> GetCostByMaintenanceType()
        {
            return MaintenanceItems
                .GroupBy(i => i.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(i => i.Status == "Completed" ? i.ActualCost : i.ProjectedCost)
                );
        }

        // Method to check maintenance due within a specific period
        public List<MaintenanceItem> GetMaintenanceDueSoon(int days)
        {
            DateTime cutoffDate = DateTime.Now.AddDays(days);
            return MaintenanceItems
                .Where(i => i.Status == "Pending" && i.Date <= cutoffDate)
                .OrderBy(i => i.Date)
                .ToList();
        }
    }
}

