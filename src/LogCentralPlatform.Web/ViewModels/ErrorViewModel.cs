using System;

namespace LogCentralPlatform.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public Exception Exception { get; set; }
        public string StackTrace { get; set; }
        public bool ShowStackTrace { get; set; }
    }
}