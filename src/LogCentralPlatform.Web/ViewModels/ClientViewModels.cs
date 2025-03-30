using System;
using System.Collections.Generic;

namespace LogCentralPlatform.Web.ViewModels
{
    public class ClientListViewModel
    {
        public List<ClientViewModel> Clients { get; set; } = new List<ClientViewModel>();
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
    }

    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ServicesCount { get; set; }
    }

    public class ClientDetailsViewModel
    {
        public ClientViewModel Client { get; set; }
        public List<ServiceViewModel> Services { get; set; } = new List<ServiceViewModel>();
        public List<LogEntryViewModel> RecentLogs { get; set; } = new List<LogEntryViewModel>();
    }
}