using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogLevel = LogCentralPlatform.Core.Entities.LogLevel;

namespace LogCentralPlatform.Web.ViewModels
{
    public class LogListViewModel
    {
        public List<LogEntrySummary> Logs { get; set; } = new List<LogEntrySummary>();
        public int TotalLogs { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public int DebugCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public string LevelFilter { get; set; }
        public string ServiceFilter { get; set; }
        public string ClientFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> AvailableLevels { get; set; } = new List<string>();
        public List<string> AvailableServices { get; set; } = new List<string>();
        public List<string> AvailableClients { get; set; } = new List<string>();
        
        // Propriétés supplémentaires présentes dans la vue
        public string SearchText { get; set; }
        public string ServiceIdFilter { get; set; }
        public string ClientIdFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int TotalCount => TotalLogs;
    }

    public class LogEntrySummary
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Context { get; set; }
    }

    public class LogEntryViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string FormattedTimestamp => Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        public LogLevel Level { get; set; }
        public string LevelClass => GetLevelClass(Level.ToString());
        public string Message { get; set; }
        public string Category { get; set; }
        public string StackTrace { get; set; }
        public string AdditionalData { get; set; }
        public string ServiceName { get; set; }
        public string ClientName { get; set; }
        public int ServiceId { get; set; }
        public int ClientId { get; set; }
        public string ServiceVersion { get; set; }
        public string MachineName { get; set; }
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public List<LogAnalysisResultViewModel> AnalysisResults { get; set; } = new List<LogAnalysisResultViewModel>();
        public List<LogEntrySummary> RelatedLogs { get; set; } = new List<LogEntrySummary>();
        
        // Propriétés additionnelles pour la vue
        public string Environment { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
        public bool AnalyzedByAI { get; set; }
        public string AIAnalysisResult { get; set; }
        public string ExceptionDetails { get; set; }

        private string GetLevelClass(string level)
        {
            return level?.ToLower() switch
            {
                "error" or "critical" or "fatal" => "text-danger",
                "warning" => "text-warning",
                "information" or "info" => "text-info",
                "debug" or "trace" => "text-secondary",
                _ => "text-dark"
            };
        }
    }

    public class LogAnalysisResultViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AnalysisType { get; set; }
        public string Result { get; set; }
        public double Confidence { get; set; }
        public List<string> SuggestedActions { get; set; } = new List<string>();
        public Dictionary<string, string> AnalysisData { get; set; } = new Dictionary<string, string>();
    }
}