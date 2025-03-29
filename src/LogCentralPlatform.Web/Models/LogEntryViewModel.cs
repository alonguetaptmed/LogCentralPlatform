using System;
using System.Collections.Generic;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Web.Models
{
    /// <summary>
    /// ViewModel pour une entrée de log.
    /// </summary>
    public class LogEntryViewModel
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
        /// Obtient une couleur CSS correspondant au niveau de log.
        /// </summary>
        public string LevelColorClass
        {
            get
            {
                return Level switch
                {
                    LogLevel.Critical => "bg-danger text-white",
                    LogLevel.Error => "bg-danger text-white",
                    LogLevel.Warning => "bg-warning",
                    LogLevel.Information => "bg-info",
                    LogLevel.Debug => "bg-secondary text-white",
                    _ => "bg-light"
                };
            }
        }

        /// <summary>
        /// Obtient un badge correspondant au niveau de log.
        /// </summary>
        public string LevelBadgeClass
        {
            get
            {
                return Level switch
                {
                    LogLevel.Critical => "badge bg-danger",
                    LogLevel.Error => "badge bg-danger",
                    LogLevel.Warning => "badge bg-warning",
                    LogLevel.Information => "badge bg-info",
                    LogLevel.Debug => "badge bg-secondary",
                    _ => "badge bg-light"
                };
            }
        }
    }

    /// <summary>
    /// ViewModel pour la liste des logs avec pagination.
    /// </summary>
    public class LogListViewModel
    {
        /// <summary>
        /// Liste des logs.
        /// </summary>
        public List<LogEntryViewModel> Logs { get; set; } = new List<LogEntryViewModel>();

        /// <summary>
        /// Nombre total de logs.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Page actuelle.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Nombre de logs par page.
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Nombre total de pages.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Indique s'il existe une page précédente.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Indique s'il existe une page suivante.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Filtre par niveau de log.
        /// </summary>
        public LogLevel? LevelFilter { get; set; }

        /// <summary>
        /// Filtre par service.
        /// </summary>
        public Guid? ServiceIdFilter { get; set; }

        /// <summary>
        /// Filtre par client.
        /// </summary>
        public Guid? ClientIdFilter { get; set; }

        /// <summary>
        /// Texte de recherche.
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// Date de début pour le filtre.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Date de fin pour le filtre.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}