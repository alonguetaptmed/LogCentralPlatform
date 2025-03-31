using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogCentralPlatform.Web.ViewModels
{
    public class SettingsViewModel
    {
        [Required(ErrorMessage = "L'URL de base de l'API est requise")]
        [Display(Name = "URL de base de l'API")]
        public string ApiBaseUrl { get; set; }

        [Required(ErrorMessage = "L'intervalle de rapport par défaut est requis")]
        [Range(1, 1440, ErrorMessage = "L'intervalle doit être compris entre 1 et 1440 minutes")]
        [Display(Name = "Intervalle de rapport par défaut (minutes)")]
        public int DefaultReportingInterval { get; set; }

        [Display(Name = "Activer l'analyse IA")]
        public bool EnableAiAnalysis { get; set; }

        [Display(Name = "Destinataires des notifications par e-mail")]
        public string EmailNotificationRecipients { get; set; }

        [Display(Name = "Activer les notifications par e-mail")]
        public bool EnableEmailNotifications { get; set; }

        [Display(Name = "Niveau de log minimum pour les notifications")]
        public string MinimumLogLevelForNotifications { get; set; }

        [Display(Name = "Délai d'expiration de session (minutes)")]
        [Range(1, 1440, ErrorMessage = "Le délai doit être compris entre 1 et 1440 minutes")]
        public int SessionTimeoutMinutes { get; set; }
        
        // Propriétés supplémentaires requises par les vues
        public string ApplicationName { get; set; }
        public string ApplicationUrl { get; set; }
        public string ApplicationVersion { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int TotalServices { get; set; }
        public int TotalLogs { get; set; }
        public string AdminEmail { get; set; }
        public string TimeZone { get; set; }
        public int LogRetentionDays { get; set; }
        public int MaxLogSizeMB { get; set; }
        public bool MaintenanceModeEnabled { get; set; }
        
        // Classes imbriquées pour les paramètres avancés
        public AISettingsModel AISettings { get; set; } = new AISettingsModel();
        public NotificationSettingsModel NotificationSettings { get; set; } = new NotificationSettingsModel();
    }
    
    public class AISettingsModel
    {
        public bool Enabled { get; set; }
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public int MaxTokens { get; set; }
        public double Temperature { get; set; }
        public int MaxResponseTime { get; set; }
        public bool AutoAnalyzeErrors { get; set; }
        public bool EnableCodeAccess { get; set; }
    }
    
    public class NotificationSettingsModel
    {
        public bool EmailEnabled { get; set; }
        public bool SlackEnabled { get; set; }
        public bool WebhookEnabled { get; set; }
        public string EmailRecipients { get; set; }
        public string SlackWebhookUrl { get; set; }
        public string CustomWebhookUrl { get; set; }
        public string NotificationLevels { get; set; }
        
        // Options pour les notifications
        public bool NotifyOnCritical { get; set; }
        public bool NotifyOnError { get; set; }
        public bool NotifyOnWarning { get; set; }
        public bool NotifyOnServiceDown { get; set; }
    }
}