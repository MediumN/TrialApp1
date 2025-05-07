using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TrialApp1.Pages.Technician
{
    public class InstallationMaintenanceModel : PageModel
    {
        [BindProperty]
        public string TaskId { get; set; }

        [BindProperty]
        public string TaskType { get; set; }

        [BindProperty]
        public string Notes { get; set; }

        [BindProperty]
        public string Status { get; set; }

        public List<InstallationTask> InstallationTasks { get; set; }
        public List<MaintenanceTask> MaintenanceTasks { get; set; }

        private static List<InstallationTask> _installationMockData = new()
        {
            new InstallationTask { Id = "1", ClientName = "Thuli Ndlovu", Address = "123 Solar St, Durban", SystemType = "Hybrid Solar", ScheduledDate = DateTime.Today.AddDays(1), Status = "Pending" },
            new InstallationTask { Id = "2", ClientName = "Peter Moyo", Address = "456 Green Ave, Joburg", SystemType = "Grid-Tied", ScheduledDate = DateTime.Today.AddDays(3), Status = "In Progress" }
        };

        private static List<MaintenanceTask> _maintenanceMockData = new()
        {
            new MaintenanceTask { Id = "3", SystemId = "SYS-101", ReportedIssue = "Battery not charging", LastServiceDate = DateTime.Today.AddDays(-30), Priority = "Urgent" },
            new MaintenanceTask { Id = "4", SystemId = "SYS-202", ReportedIssue = "Inverter error", LastServiceDate = DateTime.Today.AddDays(-60), Priority = "High" }
        };

        public void OnGet()
        {
            InstallationTasks = _installationMockData;
            MaintenanceTasks = _maintenanceMockData;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data submitted.";
                return RedirectToPage();
            }

            if (TaskType == "install")
            {
                var task = _installationMockData.FirstOrDefault(t => t.Id == TaskId);
                if (task != null)
                {
                    task.Status = "Completed";
                    if (!string.IsNullOrWhiteSpace(Notes))
                    {
                        task.Notes = Notes;
                    }
                    TempData["SuccessMessage"] = "Installation task updated.";
                }
            }
            else if (TaskType == "maint")
            {
                var task = _maintenanceMockData.FirstOrDefault(t => t.Id == TaskId);
                if (task != null)
                {
                    task.Status = Status;
                    task.Notes = Notes;
                    TempData["SuccessMessage"] = "Maintenance task updated.";
                }
            }

            return RedirectToPage();
        }

        public class InstallationTask
        {
            public string Id { get; set; }
            public string ClientName { get; set; }
            public string Address { get; set; }
            public string SystemType { get; set; }
            public DateTime ScheduledDate { get; set; }
            public string Status { get; set; }
            public string Notes { get; set; }
        }

        public class MaintenanceTask
        {
            public string Id { get; set; }
            public string SystemId { get; set; }
            public string ReportedIssue { get; set; }
            public DateTime LastServiceDate { get; set; }
            public string Priority { get; set; }
            public string Status { get; set; }
            public string Notes { get; set; }
        }
    }
}
