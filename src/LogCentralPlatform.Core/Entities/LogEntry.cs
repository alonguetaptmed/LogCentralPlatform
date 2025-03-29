using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LogCentralPlatform.Core.Entities
{
    /// <summary>
    /// Représente une entrée de log dans le système.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Identifiant unique de l'entrée de log.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Horodatage de l'événement de log.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Niveau de gravité du log (Information, Warning, Error, Critical, etc.).
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Message principal du log.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Identifiant du service qui a généré le log.
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Nom du service qui a généré le log.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Version du service.
        /// </summary>
        public string ServiceVersion { get; set; } = string.Empty;

        /// <summary>
        /// Environnement d'exécution (Production, Staging, Development, etc.).
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Catégorie ou composant spécifique à l'origine du log.
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// ID du client associé au service.
        /// </summary>
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Nom du client associé au service.
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// Exception sérialisée (si applicable).
        /// </summary>
        public string? ExceptionDetails { get; set; }

        /// <summary>
        /// Pile d'appel de l'exception (si applicable).
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Identifiant de corrélation pour lier plusieurs logs à un même événement.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Données contextuelles additionnelles au format JSON.
        /// </summary>
        public string? ContextData { get; set; }

        /// <summary>
        /// Indique si le log contient des informations sensibles qui devraient être masquées/chiffrées.
        /// </summary>
        public bool ContainsSensitiveData { get; set; }

        /// <summary>
        /// Adresse IP de la machine qui a généré le log.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Indique si ce log a été traité par l'IA.
        /// </summary>
        public bool AnalyzedByAI { get; set; }

        /// <summary>
        /// Commentaires ou résultats d'analyse de l'IA.
        /// </summary>
        public string? AIAnalysisResult { get; set; }

        /// <summary>
        /// Horodatage de la réception du log par la plateforme.
        /// </summary>
        public DateTime ReceivedAt { get; set; }

        /// <summary>
        /// Métadonnées supplémentaires liées au log.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; set; }
    }
}