namespace LogCentralPlatform.Web.ViewModels
{
    public class StatCardViewModel
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; } // primary, success, warning, danger, info
        public string Subtitle { get; set; }
        public string Trend { get; set; } // up, down, stable
        public string TrendValue { get; set; }
        public string RedirectUrl { get; set; }
    }
}