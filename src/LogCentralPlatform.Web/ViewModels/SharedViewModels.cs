using System;
using System.Collections.Generic;

namespace LogCentralPlatform.Web.ViewModels
{
    public class AlertViewModel
    {
        public string Type { get; set; } // success, warning, danger, info
        public string Message { get; set; }
        public bool Dismissible { get; set; } = true;
        public int DisplayDuration { get; set; } = 5000; // milliseconds, 0 for no auto-dismiss
    }

    public class StatCardViewModel
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; } // primary, success, warning, danger, info
        public string Trend { get; set; } // up, down, stable
        public string TrendValue { get; set; }
        public string Link { get; set; }
    }

    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string RouteAction { get; set; }
        public Dictionary<string, string> RouteParams { get; set; } = new Dictionary<string, string>();
    }

    public class DashboardViewModel
    {
        public List<StatCardViewModel> Stats { get; set; } = new List<StatCardViewModel>();
        public List<AlertViewModel> Alerts { get; set; } = new List<AlertViewModel>();
        public List<ServiceSummary> RecentlyOfflineServices { get; set; } = new List<ServiceSummary>();
        public List<LogSummaryViewModel> RecentErrors { get; set; } = new List<LogSummaryViewModel>();
        public Dictionary<string, int> LogLevelDistribution { get; set; } = new Dictionary<string, int>();
        public Dictionary<DateTime, int> LogsTimeline { get; set; } = new Dictionary<DateTime, int>();
    }
}