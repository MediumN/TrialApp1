using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class NotificationsModel : PageModel
{
    public List<Notification> Notifications { get; set; } = new();
    public Dictionary<string, List<Notification>> GroupedNotifications { get; set; }

    [BindProperty(SupportsGet = true)] public string? FilterType { get; set; }
    [BindProperty(SupportsGet = true)] public string? FilterSeverity { get; set; }
    [BindProperty(SupportsGet = true)] public string? FilterRead { get; set; }

    public void OnGet()
    {
        var allNotifications = GetMockNotifications();

        // Filtering logic
        Notifications = allNotifications
            .Where(n =>
                (string.IsNullOrEmpty(FilterType) || n.Type == FilterType) &&
                (string.IsNullOrEmpty(FilterSeverity) || n.Severity.ToString() == FilterSeverity) &&
                (string.IsNullOrEmpty(FilterRead) || (FilterRead == "Unread" && !n.IsRead) || (FilterRead == "Read" && n.IsRead))
            )
            .OrderByDescending(n => n.Timestamp)
            .ToList();

        GroupedNotifications = Notifications
            .GroupBy(n => n.Type switch
            {
                "SystemAlert" => "System Alerts",
                "Maintenance" => "Maintenance Reminders",
                "Performance" => "Performance Insights",
                _ => "Other"
            })
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    private List<Notification> GetMockNotifications()
    {
        return new List<Notification>
        {
            new() { Id = "1", Type = "SystemAlert", Severity = Severity.Error, Title = "Inverter Overload", Timestamp = DateTime.Now.AddMinutes(-10), Summary = "The inverter has exceeded its load capacity.", Icon = "⚠️", IsRead = false },
            new() { Id = "2", Type = "Maintenance", Severity = Severity.Warning, Title = "Battery Maintenance Due", Timestamp = DateTime.Now.AddHours(-2), Summary = "Time to check battery health and water levels.", Icon = "🔧", IsRead = true },
            new() { Id = "3", Type = "Performance", Severity = Severity.Info, Title = "Solar Efficiency Update", Timestamp = DateTime.Now.AddDays(-1), Summary = "Solar panels performed at 87% efficiency yesterday.", Icon = "🔋", IsRead = false },
            new() { Id = "4", Type = "SystemAlert", Severity = Severity.Warning, Title = "Voltage Fluctuation", Timestamp = DateTime.Now.AddMinutes(-30), Summary = "Detected unstable voltage input.", Icon = "⚠️", IsRead = false },
        };
    }

    public class Notification
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public Severity Severity { get; set; }
        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
        public bool IsRead { get; set; }
    }

    public enum Severity
    {
        Info,
        Warning,
        Error
    }
}
