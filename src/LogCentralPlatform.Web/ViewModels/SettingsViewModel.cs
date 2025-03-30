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
    }
}