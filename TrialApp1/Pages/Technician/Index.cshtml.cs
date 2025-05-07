using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace TrialApp1.Pages.Technician
{
    public class IndexModel : PageModel
    {
        public int AssignedTasks { get; set; }
        public int PendingInstallations { get; set; }
        public int ScheduledMaintenance { get; set; }
        public List<DeviceAlert> DeviceAlerts { get; set; }
        public List<ScheduledTask> UpcomingSchedule { get; set; }
        public List<Notification> Notifications { get; set; }

        public void OnGet()
        {
            // Simulated data
            AssignedTasks = 12;
            PendingInstallations = 5;
            ScheduledMaintenance = 7;

            DeviceAlerts = new List<DeviceAlert>
            {
                new DeviceAlert { DeviceName = "Inverter-X100", AlertType = "Offline", Severity = "Critical", Description = "Device not responding" },
                new DeviceAlert { DeviceName = "Battery-B200", AlertType = "Low Battery", Severity = "Warning", Description = "Battery level at 15%" },
                new DeviceAlert { DeviceName = "Sensor-A1", AlertType = "Error Code 503", Severity = "Critical", Description = "Internal sensor malfunction" }
            };

            UpcomingSchedule = new List<ScheduledTask>
            {
                new ScheduledTask { Date = DateTime.Today.AddDays(1), Time = "09:00 AM", ClientName = "GreenTech SA", Location = "Durban", TaskType = "Installation" },
                new ScheduledTask { Date = DateTime.Today.AddDays(2), Time = "11:00 AM", ClientName = "SolarGrid", Location = "Pinetown", TaskType = "Maintenance" },
                new ScheduledTask { Date = DateTime.Today.AddDays(3), Time = "03:00 PM", ClientName = "EcoSystems", Location = "Umhlanga", TaskType = "Diagnostics" }
            };

            Notifications = new List<Notification>
            {
                new Notification { Title = "New Assignment", Message = "You have been assigned to a new maintenance task at SolarGrid." },
                new Notification { Title = "System Update", Message = "Firmware v2.1.5 is available for all inverters." },
                new Notification { Title = "Reminder", Message = "Submit your daily report by 5 PM today." }
            };
        }

        public class DeviceAlert
        {
            public string DeviceName { get; set; }
            public string AlertType { get; set; }
            public string Severity { get; set; }
            public string Description { get; set; }
        }

        public class ScheduledTask
        {
            public DateTime Date { get; set; }
            public string Time { get; set; }
            public string ClientName { get; set; }
            public string Location { get; set; }
            public string TaskType { get; set; }
        }

        public class Notification
        {
            public string Title { get; set; }
            public string Message { get; set; }
        }
    }
}
