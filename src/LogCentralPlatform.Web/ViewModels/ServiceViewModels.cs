using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogCentralPlatform.Web.ViewModels
{
    public class ServiceListViewModel
    {
        public List<ServiceSummary> Services { get; set; } = new List<ServiceSummary>();
        public int TotalServices { get; set; }
        public int OnlineServices { get; set; }
        public int OfflineServices { get; set; }
        public int WarningServices { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public string ClientFilter { get; set; }
        public string StatusFilter { get; set; }
        
        // Propriétés supplémentaires requises par les vues
        public int CurrentPage { get; set; } = 1;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int TotalCount => TotalServices;
        public string SearchText { get; set; }
        public string Status { get; set; }
        public int ClientId { get; set; }
        public List<ClientSummary> Clients { get; set; } = new List<ClientSummary>();
    }

    public class ServiceSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public string Status { get; set; }
        public int ErrorCount24h { get; set; }
        public string ApiKey { get; set; }
        
        // Propriétés supplémentaires requises par les vues
        public string Description { get; set; }
        public string Environment { get; set; }
        public DateTime LastActivity { get; set; }
    }

    public class ServiceDetailsViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le nom du service est requis")]
        [Display(Name = "Nom du service")]
        public string Name { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Display(Name = "Version")]
        public string Version { get; set; }
        
        [Required(ErrorMessage = "L'ID du client est requis")]
        public int ClientId { get; set; }
        
        [Display(Name = "Client")]
        public string ClientName { get; set; }
        
        [Display(Name = "Intervalle de reporting (minutes)")]
        [Range(1, 1440, ErrorMessage = "L'intervalle doit être compris entre 1 et 1440 minutes")]
        public int ReportingInterval { get; set; }
        
        [Display(Name = "URL de callback")]
        public string CallbackUrl { get; set; }
        
        [Display(Name = "Clé API")]
        public string ApiKey { get; set; }
        
        public DateTime LastHeartbeat { get; set; }
        
        [Display(Name = "État")]
        public string Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public List<LogSummaryViewModel> RecentLogs { get; set; } = new List<LogSummaryViewModel>();
        
        public ServiceStatsViewModel Stats { get; set; } = new ServiceStatsViewModel();
        
        // Propriétés supplémentaires requises par les vues
        public string Environment { get; set; }
        public int MonitoringInterval { get; set; }
        public bool EnableAIAnalysis { get; set; }
        public int LogCount { get; set; }
        public int ErrorCount { get; set; }
        public double UptimePercentage { get; set; }
        public List<AlertViewModel> Alerts { get; set; } = new List<AlertViewModel>();
    }

    public class LogSummaryViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Context { get; set; }
    }

    public class ServiceStatsViewModel
    {
        public int TotalLogsCount { get; set; }
        public int ErrorsLast24h { get; set; }
        public int WarningsLast24h { get; set; }
        public int InfoLast24h { get; set; }
        public double UptimePercentage { get; set; }
        public DateTime? LastErrorTime { get; set; }
        public TimeSpan AverageResponseTime { get; set; }
    }
}