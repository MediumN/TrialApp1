using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace TrialApp1.Pages
{
    public class BatteryManagementModel : PageModel
    {
        [BindProperty]
        public BatteryBank BatteryBank { get; set; }

        public bool HasCriticalAlert { get; private set; }
        public string AlertMessage { get; private set; }
        public string BatteryStatusColor { get; private set; }
        public string BatteryHealthTextColor { get; private set; }
        public string MaintenanceProgressBarColor { get; private set; }
        public int MaintenanceProgressPercentage { get; private set; }
        public int DaysSinceLastMaintenance { get; private set; }
        public string DaysUntilNextMaintenanceMsg { get; private set; }

        public void OnGet()
        {
            // Initialize battery data (in a real app, this would come from a service or database)
            InitializeBatteryData();

            // Calculate derived values and status indicators
            CalculateDerivedValues();
        }

        public IActionResult OnPostRunDiagnostics()
        {
            // Simulate fetching up-to-date data
            InitializeBatteryData();
            CalculateDerivedValues();

            // Simulate running diagnostics and returning results
            BatteryBank.LastMaintenanceDate = DateTime.Now; // Update maintenance date
            BatteryBank.BatteryHealth = GetRandomHealthStatus(); // Update health based on simulated diagnostics

            TempData["SuccessMessage"] = "Battery diagnostics completed successfully. Battery health: " + BatteryBank.BatteryHealth;
            return RedirectToPage();
        }

        public IActionResult OnPostSaveSettings()
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, repopulate the data
                InitializeBatteryData();
                CalculateDerivedValues();
                return Page();
            }

            // Simulate saving the settings (in a real app, this would call a service)
            // Here we just acknowledge the change in energy saving mode
            string modeStatus = BatteryBank.EnergySavingMode ? "enabled" : "disabled";
            TempData["SuccessMessage"] = $"Battery settings saved successfully. Energy saving mode {modeStatus}.";

            return RedirectToPage();
        }

        private void InitializeBatteryData()
        {
            // Example data for the battery bank - in a real application, this would come from a database or service
            BatteryBank = new BatteryBank
            {
                Name = "Main Solar Battery Bank",
                ChargeLevel = 78,
                ChargingStatus = "Charging", // Options: "Charging", "Discharging", "Idle"
                Voltage = 48.3,
                Current = 12.5,
                Temperature = 27.5,
                EstimatedRuntime = "12:42",
                BatteryHealth = "Good", // Options: "Good", "Warning", "Replace Soon"
                LastMaintenanceDate = DateTime.Now.AddDays(-75),
                InstallationDate = DateTime.Now.AddYears(-2).AddMonths(-4),
                WarrantyDate = DateTime.Now.AddYears(5).AddMonths(-4),
                Capacity = 13.5,
                EnergySavingMode = true
            };

            // Set critical alert if needed (temperature too high or voltage too low)
            if (BatteryBank.Temperature > 40)
            {
                HasCriticalAlert = true;
                AlertMessage = $"Battery temperature critical: {BatteryBank.Temperature}°C exceeds safe operating range.";
            }
            else if (BatteryBank.Voltage < 42)
            {
                HasCriticalAlert = true;
                AlertMessage = $"Battery voltage critically low: {BatteryBank.Voltage}V. Connect to charging source immediately.";
            }
            else
            {
                HasCriticalAlert = false;
                AlertMessage = string.Empty;
            }
        }

        private void CalculateDerivedValues()
        {
            // Determine battery level color based on charge percentage
            if (BatteryBank.ChargeLevel > 70)
                BatteryStatusColor = "#28a745"; // Green
            else if (BatteryBank.ChargeLevel > 30)
                BatteryStatusColor = "#ffc107"; // Yellow/Amber
            else
                BatteryStatusColor = "#dc3545"; // Red

            // Set health status text color
            switch (BatteryBank.BatteryHealth)
            {
                case "Good":
                    BatteryHealthTextColor = "text-success";
                    break;
                case "Warning":
                    BatteryHealthTextColor = "text-warning";
                    break;
                case "Replace Soon":
                    BatteryHealthTextColor = "text-danger";
                    break;
                default:
                    BatteryHealthTextColor = "text-muted";
                    break;
            }

            // Calculate maintenance information
            DaysSinceLastMaintenance = (int)(DateTime.Now - BatteryBank.LastMaintenanceDate).TotalDays;

            // Assuming maintenance is recommended every 90 days
            const int RecommendedMaintenanceInterval = 90;
            int daysUntilNextMaintenance = RecommendedMaintenanceInterval - DaysSinceLastMaintenance;

            // Calculate maintenance progress percentage (how far through the maintenance cycle)
            MaintenanceProgressPercentage = Math.Min(100, (int)((double)DaysSinceLastMaintenance / RecommendedMaintenanceInterval * 100));

            // Set maintenance progress bar color
            if (MaintenanceProgressPercentage < 70)
                MaintenanceProgressBarColor = "bg-success";
            else if (MaintenanceProgressPercentage < 90)
                MaintenanceProgressBarColor = "bg-warning";
            else
                MaintenanceProgressBarColor = "bg-danger";

            // Message about days until next maintenance
            if (daysUntilNextMaintenance > 0)
                DaysUntilNextMaintenanceMsg = $"{daysUntilNextMaintenance} days until next recommended maintenance";
            else
                DaysUntilNextMaintenanceMsg = "Maintenance recommended now";
        }

        // Helper method to get random health status for diagnostic demo
        private string GetRandomHealthStatus()
        {
            // This simulates diagnostic results - in a real app, this would be based on actual measurements
            string[] statuses = { "Good", "Warning", "Good", "Good", "Good" }; // Weighted toward "Good"
            Random rand = new Random();
            return statuses[rand.Next(statuses.Length)];
        }
    }

    public class BatteryBank
    {
        public string Name { get; set; } = string.Empty;
        public int ChargeLevel { get; set; }
        public string ChargingStatus { get; set; } = string.Empty;
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double Temperature { get; set; }
        public string EstimatedRuntime { get; set; } = string.Empty;
        public string BatteryHealth { get; set; } = string.Empty;
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime WarrantyDate { get; set; }
        public double Capacity { get; set; }
        public bool EnergySavingMode { get; set; }
    }
}