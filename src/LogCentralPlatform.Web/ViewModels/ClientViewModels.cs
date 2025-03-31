using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogCentralPlatform.Web.ViewModels
{
    public class ClientListViewModel
    {
        public List<ClientSummary> Clients { get; set; } = new List<ClientSummary>();
        public int TotalClients { get; set; }
        public int ActiveClients { get; set; }
        public int InactiveClients { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        
        // Propriétés supplémentaires requises par les vues
        public int CurrentPage { get; set; } = 1;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int TotalCount => TotalClients;
        public string SearchText { get; set; }
        public string Status { get; set; }
        public string SortBy { get; set; }
    }

    public class ClientSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public int ServiceCount { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsActive { get; set; }
        
        // Propriétés supplémentaires requises par les vues
        public string Code { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public DateTime LastAccessAt { get; set; }
    }

    public class ClientDetailsViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le nom du client est requis")]
        [Display(Name = "Nom du client")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "L'adresse e-mail de contact est requise")]
        [EmailAddress(ErrorMessage = "Format d'adresse e-mail invalide")]
        [Display(Name = "E-mail de contact")]
        public string ContactEmail { get; set; }
        
        [Display(Name = "Téléphone")]
        public string Phone { get; set; }
        
        [Display(Name = "Adresse")]
        public string Address { get; set; }
        
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastModifiedAt { get; set; }
        
        [Display(Name = "Actif")]
        public bool IsActive { get; set; }
        
        public List<ServiceSummary> Services { get; set; } = new List<ServiceSummary>();
        
        public List<ClientLogSummary> RecentLogs { get; set; } = new List<ClientLogSummary>();
        
        // Propriétés supplémentaires requises par les vues
        public string Code { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public int ServiceCount { get; set; }
        public int UserCount { get; set; }
        public int LogCount { get; set; }
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }

    public class ClientLogSummary
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string ServiceName { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
    
    // Nouveau ViewModel pour les utilisateurs référencés dans ClientDetailsViewModel
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
    }
}