using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace TrialApp1.Pages
{
    public class EnergyAnalyticsModel : PageModel
    {
        public List<string> DailyLabels { get; set; }
        public List<double> DailyValues { get; set; }

        public List<string> WeeklyLabels { get; set; }
        public List<double> WeeklyValues { get; set; }

        public double Solar { get; set; }
        public double Battery { get; set; }
        public double Grid { get; set; }

        public double TotalEnergyUsed { get; set; }
        public string PeakUsageTime { get; set; }
        public double CostEstimate { get; set; }
        public double CO2Saved { get; set; }

        public void OnGet()
        {
            // Simulate daily data
            DailyLabels = Enumerable.Range(0, 24).Select(i => $"{i}:00").ToList();
            DailyValues = DailyLabels.Select(_ => Random.Shared.NextDouble() * 2).ToList();

            // Simulate weekly data
            WeeklyLabels = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            WeeklyValues = WeeklyLabels.Select(_ => Random.Shared.NextDouble() * 10).ToList();

            // Simulate pie chart values
            Solar = 55;
            Battery = 30;
            Grid = 15;

            // Simulate KPI values
            TotalEnergyUsed = DailyValues.Sum() + WeeklyValues.Sum();
            PeakUsageTime = "18:00";
            CostEstimate = Math.Round(TotalEnergyUsed * 1.45, 2); // Assume R1.45/kWh
            CO2Saved = Math.Round(TotalEnergyUsed * 0.75, 2);     // Assume 0.75 kg/kWh
        }
    }
}
