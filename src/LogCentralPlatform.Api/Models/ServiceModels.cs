using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Api.Models
{
    /// <summary>
    /// Modèle pour la création d'un service.
    /// </summary>
    public class CreateServiceRequest
    {
        /// <summary>
        /// Nom du service.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description du service.
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Version actuelle du service.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Type de service (application web, service Windows, etc.).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; } = string.Empty;

        /// <summary>
        /// Identifiant du client propriétaire du service.
        /// </summary>
        [Required]
        public Guid ClientId { get; set; }

        /// <summary>
        /// Environnement de déploiement (Production, Staging, Development, etc.).
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Intervalle minimum attendu entre les rapports de logs (en minutes).
        /// </summary>
        [Range(1, int.MaxValue)]
        public int ReportingIntervalMinutes { get; set; } = 60;

        /// <summary>
        /// Indique si des alertes doivent être envoyées pour ce service.
        /// </summary>
        public bool AlertsEnabled { get; set; } = true;

        /// <summary>
        /// Niveau minimum de log pour déclencher une alerte.
        /// </summary>
        public Core.Entities.LogLevel AlertThreshold { get; set; } = Core.Entities.LogLevel.Error;

        /// <summary>
        /// Destinataires des alertes par e-mail.
        /// </summary>
        public List<string> AlertEmailRecipients { get; set; } = new List<string>();

        /// <summary>
        /// URL du webhook pour les notifications.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Métadonnées supplémentaires liées au service.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Chemin relatif vers le code source, si disponible.
        /// </summary>
        public string? SourceCodePath { get; set; }
    }

    /// <summary>
    /// Modèle pour la mise à jour d'un service.
    /// </summary>
    public class UpdateServiceRequest
    {
        /// <summary>
        /// Nom du service.
        /// </summary>
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }

        /// <summary>
        /// Description du service.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Version actuelle du service.
        /// </summary>
        [StringLength(20)]
        public string? Version { get; set; }

        /// <summary>
        /// Type de service (application web, service Windows, etc.).
        /// </summary>
        [StringLength(50)]
        public string? ServiceType { get; set; }

        /// <summary>
        /// Environnement de déploiement (Production, Staging, Development, etc.).
        /// </summary>
        [StringLength(50)]
        public string? Environment { get; set; }

        /// <summary>
        /// Intervalle minimum attendu entre les rapports de logs (en minutes).
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? ReportingIntervalMinutes { get; set; }

        /// <summary>
        /// Indique si des alertes doivent être envoyées pour ce service.
        /// </summary>
        public bool? AlertsEnabled { get; set; }

        /// <summary>
        /// Niveau minimum de log pour déclencher une alerte.
        /// </summary>
        public Core.Entities.LogLevel? AlertThreshold { get; set; }

        /// <summary>
        /// Destinataires des alertes par e-mail.
        /// </summary>
        public List<string>? AlertEmailRecipients { get; set; }

        /// <summary>
        /// URL du webhook pour les notifications.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Métadonnées supplémentaires liées au service.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Chemin relatif vers le code source, si disponible.
        /// </summary>
        public string? SourceCodePath { get; set; }
    }

    /// <summary>
    /// DTO pour un service enregistré.
    /// </summary>
    public class ServiceDto
    {
        /// <summary>
        /// Identifiant unique du service.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du service.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description du service.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Version actuelle du service.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Type de service (application web, service Windows, etc.).
        /// </summary>
        public string ServiceType { get; set; } = string.Empty;

        /// <summary>
        /// Clé API unique pour l'authentification du service.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Date de création de l'enregistrement du service.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date de dernière mise à jour de l'enregistrement.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Dernière fois que le service a envoyé un log.
        /// </summary>
        public DateTime? LastLogReceivedAt { get; set; }

        /// <summary>
        /// Identifiant du client propriétaire du service.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Nom du client propriétaire du service.
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Environnement de déploiement (Production, Staging, Development, etc.).
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Intervalle minimum attendu entre les rapports de logs (en minutes).
        /// </summary>
        public int ReportingIntervalMinutes { get; set; }

        /// <summary>
        /// Indique si le service est actuellement actif.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Indique si le service est considéré comme étant en ligne selon son dernier rapport.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Indique si des alertes doivent être envoyées pour ce service.
        /// </summary>
        public bool AlertsEnabled { get; set; }

        /// <summary>
        /// Niveau minimum de log pour déclencher une alerte.
        /// </summary>
        public Core.Entities.LogLevel AlertThreshold { get; set; }

        /// <summary>
        /// Destinataires des alertes par e-mail.
        /// </summary>
        public List<string> AlertEmailRecipients { get; set; } = new List<string>();

        /// <summary>
        /// URL du webhook pour les notifications.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Métadonnées supplémentaires liées au service.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Chemin relatif vers le code source, si disponible.
        /// </summary>
        public string? SourceCodePath { get; set; }
    }

    /// <summary>
    /// Modèle pour la régénération de la clé API d'un service.
    /// </summary>
    public class RegenerateApiKeyResponse
    {
        /// <summary>
        /// Nouvelle clé API générée.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Identifiant du service.
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Indique si l'opération a réussi.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message d'erreur en cas d'échec.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Modèle pour la recherche de services.
    /// </summary>
    public class SearchServicesRequest
    {
        /// <summary>
        /// Terme de recherche pour le nom ou la description.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Identifiant du client pour filtrer les services.
        /// </summary>
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Indique si les services inactifs doivent être inclus.
        /// </summary>
        public bool IncludeInactive { get; set; } = false;
    }
}