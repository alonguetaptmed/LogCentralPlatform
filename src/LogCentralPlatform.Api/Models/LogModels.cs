using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Api.Models
{
    /// <summary>
    /// Modèle pour la création d'un log via l'API.
    /// </summary>
    public class CreateLogRequest
    {
        /// <summary>
        /// Horodatage de l'événement de log.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Niveau de gravité du log (Information, Warning, Error, Critical, etc.).
        /// </summary>
        [Required]
        public LogLevel Level { get; set; }

        /// <summary>
        /// Message principal du log.
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Catégorie ou composant spécifique à l'origine du log.
        /// </summary>
        public string? Category { get; set; }

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
        /// Métadonnées supplémentaires liées au log.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; set; }
    }

    /// <summary>
    /// Modèle pour la réponse suite à la création d'un log.
    /// </summary>
    public class CreateLogResponse
    {
        /// <summary>
        /// Identifiant unique du log créé.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Horodatage de la réception du log.
        /// </summary>
        public DateTime ReceivedAt { get; set; }

        /// <summary>
        /// Indique si le log a été accepté et stocké avec succès.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message d'erreur en cas d'échec.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Modèle pour la recherche de logs.
    /// </summary>
    public class SearchLogsRequest
    {
        /// <summary>
        /// Date de début pour la recherche.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Date de fin pour la recherche.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Identifiant du service pour filtrer les logs.
        /// </summary>
        public Guid? ServiceId { get; set; }

        /// <summary>
        /// Identifiant du client pour filtrer les logs.
        /// </summary>
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Niveau de gravité minimum pour filtrer les logs.
        /// </summary>
        public LogLevel? MinLevel { get; set; }

        /// <summary>
        /// Texte à rechercher dans les messages de log.
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// Catégorie à filtrer.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Identifiant de corrélation pour filtrer les logs liés.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Nombre d'éléments à sauter (pagination).
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Nombre d'éléments à retourner (pagination).
        /// </summary>
        public int Take { get; set; } = 50;
    }

    /// <summary>
    /// Modèle pour la réponse de recherche de logs.
    /// </summary>
    public class SearchLogsResponse
    {
        /// <summary>
        /// Liste des logs correspondant aux critères.
        /// </summary>
        public List<LogEntryDto> Logs { get; set; } = new List<LogEntryDto>();

        /// <summary>
        /// Nombre total de logs correspondant aux critères (pour la pagination).
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Indique si la recherche a réussi.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message d'erreur en cas d'échec.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// DTO pour une entrée de log.
    /// </summary>
    public class LogEntryDto
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
        /// Niveau de gravité du log.
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
        /// Environnement d'exécution.
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