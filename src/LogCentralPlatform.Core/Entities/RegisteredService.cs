using System;
using System.Collections.Generic;

namespace LogCentralPlatform.Core.Entities
{
    /// <summary>
    /// Représente un service enregistré dans la plateforme de logs.
    /// </summary>
    public class RegisteredService
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
        public int ReportingIntervalMinutes { get; set; } = 60;

        /// <summary>
        /// Indique si le service est actuellement actif.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indique si le service est considéré comme étant en ligne selon son dernier rapport.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// Indique si des alertes doivent être envoyées pour ce service.
        /// </summary>
        public bool AlertsEnabled { get; set; } = true;

        /// <summary>
        /// Niveau minimum de log pour déclencher une alerte.
        /// </summary>
        public LogLevel AlertThreshold { get; set; } = LogLevel.Error;

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
}