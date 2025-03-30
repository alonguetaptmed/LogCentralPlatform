using System;

namespace LogCentralPlatform.Web.ViewModels
{
    public class AlertViewModel
    {
        public string Type { get; set; } // success, info, warning, danger
        public string Message { get; set; }
        public bool Dismissible { get; set; } = true;
        public string Icon { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}