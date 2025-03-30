namespace LogCentralPlatform.Web.ViewModels
{
    public class SettingsViewModel
    {
        public string ApiBaseUrl { get; set; }
        public int DefaultReportingInterval { get; set; }
        public bool EnableAiAnalysis { get; set; }
        public string EmailNotificationRecipients { get; set; }
        public bool EnableEmailNotifications { get; set; }
    }
}