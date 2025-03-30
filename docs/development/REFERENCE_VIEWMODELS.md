# Référence des ViewModels

Ce document fournit une référence complète des ViewModels utilisés dans le projet LogCentralPlatform.Web.

## Authentification

### LoginViewModel

Utilisé pour la page de connexion.

```csharp
public class LoginViewModel
{
    [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
    [Display(Name = "Nom d'utilisateur")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Le mot de passe est requis")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; }

    [Display(Name = "Se souvenir de moi")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
}
```

## Administration

### SettingsViewModel

Utilisé pour la page des paramètres d'administration.

```csharp
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
```

## Gestion des Clients

### ClientListViewModel

Utilisé pour la page de liste des clients.

```csharp
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
}

public class ClientSummary
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public int ServiceCount { get; set; }
    public DateTime LastActivity { get; set; }
    public bool IsActive { get; set; }
}
```

### ClientDetailsViewModel

Utilisé pour la page de détail d'un client.

```csharp
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
}

public class ClientLogSummary
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string ServiceName { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
}
```

## Gestion des Services

### ServiceListViewModel

Utilisé pour la page de liste des services.

```csharp
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
}
```

### ServiceDetailsViewModel

Utilisé pour la page de détail d'un service.

```csharp
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
    
    public List<LogEntrySummary> RecentLogs { get; set; } = new List<LogEntrySummary>();
    
    public ServiceStatsViewModel Stats { get; set; } = new ServiceStatsViewModel();
}

public class LogEntrySummary
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
```

## Composants partagés

### AlertViewModel

Utilisé pour les alertes dans les vues.

```csharp
public class AlertViewModel
{
    public string Type { get; set; } // success, warning, danger, info
    public string Message { get; set; }
    public bool Dismissible { get; set; } = true;
    public int DisplayDuration { get; set; } = 5000; // milliseconds, 0 pour pas d'auto-suppression
}
```

### StatCardViewModel

Utilisé pour les cartes de statistiques dans le tableau de bord.

```csharp
public class StatCardViewModel
{
    public string Title { get; set; }
    public string Value { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; } // primary, success, warning, danger, info
    public string Trend { get; set; } // up, down, stable
    public string TrendValue { get; set; }
    public string Link { get; set; }
}
```

### PaginationViewModel

Utilisé pour la pagination dans les listes.

```csharp
public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string RouteAction { get; set; }
    public Dictionary<string, string> RouteParams { get; set; } = new Dictionary<string, string>();
}
```

### DashboardViewModel

Utilisé pour la page d'accueil du tableau de bord.

```csharp
public class DashboardViewModel
{
    public List<StatCardViewModel> Stats { get; set; } = new List<StatCardViewModel>();
    public List<AlertViewModel> Alerts { get; set; } = new List<AlertViewModel>();
    public List<ServiceSummary> RecentlyOfflineServices { get; set; } = new List<ServiceSummary>();
    public List<LogEntrySummary> RecentErrors { get; set; } = new List<LogEntrySummary>();
    public Dictionary<string, int> LogLevelDistribution { get; set; } = new Dictionary<string, int>();
    public Dictionary<DateTime, int> LogsTimeline { get; set; } = new Dictionary<DateTime, int>();
}
```

## Bonnes pratiques

1. **Validation des données** : Utilisez les annotations de validation pour toutes les propriétés de formulaire
2. **Gestion des collections** : Initialisez toujours les collections pour éviter les erreurs NullReferenceException
3. **Organisation** : Regroupez les ViewModels liés dans des fichiers par fonctionnalité
4. **Mapping** : Utilisez les helpers ou AutoMapper pour convertir entre les entités et les ViewModels