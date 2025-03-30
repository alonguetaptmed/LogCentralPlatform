using System;
using System.Collections.Generic;

namespace LogCentralPlatform.Web.ViewModels
{
    public class ServiceListViewModel
    {
        public List<ServiceViewModel> Services { get; set; } = new List<ServiceViewModel>();
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int WarningCount { get; set; }
        public int ErrorCount { get; set; }
    }

    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime LastReportTime { get; set; }
        public string Status { get; set; }  // Active, Warning, Error, Inactive
        public int ReportingIntervalMinutes { get; set; }
    }

    public class ServiceDetailsViewModel
    {
        public ServiceViewModel Service { get; set; }
        public List<LogEntryViewModel> RecentLogs { get; set; } = new List<LogEntryViewModel>();
        public Dictionary<string, int> LogLevelCounts { get; set; } = new Dictionary<string, int>();
        public List<string> AiSuggestions { get; set; } = new List<string>();
    }

    public class LogEntryViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }  // Info, Warning, Error, Critical
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string AdditionalData { get; set; }
    }
}