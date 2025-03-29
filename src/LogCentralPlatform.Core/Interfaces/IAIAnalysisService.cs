using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Core.Interfaces
{
    /// <summary>
    /// Interface pour le service d'analyse des logs par IA.
    /// </summary>
    public interface IAIAnalysisService
    {
        /// <summary>
        /// Analyse un log spécifique.
        /// </summary>
        /// <param name="logEntry">L'entrée de log à analyser.</param>
        /// <returns>Le résultat de l'analyse.</returns>
        Task<AIAnalysisResult> AnalyzeLogAsync(LogEntry logEntry);

        /// <summary>
        /// Analyse un ensemble de logs pour détecter des motifs ou tendances.
        /// </summary>
        /// <param name="logs">Les entrées de log à analyser.</param>
        /// <returns>Le résultat de l'analyse.</returns>
        Task<AIAnalysisResult> AnalyzeLogsPatternAsync(IEnumerable<LogEntry> logs);

        /// <summary>
        /// Détecte les anomalies dans les logs d'un service.
        /// </summary>
        /// <param name="serviceId">L'identifiant du service.</param>
        /// <param name="startDate">Date de début de l'analyse.</param>
        /// <param name="endDate">Date de fin de l'analyse.</param>
        /// <returns>La liste des anomalies détectées.</returns>
        Task<IEnumerable<AIAnomaly>> DetectAnomaliesAsync(Guid serviceId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Suggère des solutions pour une erreur spécifique.
        /// </summary>
        /// <param name="logId">L'identifiant du log d'erreur.</param>
        /// <param name="sourceCode">Le code source associé (optionnel).</param>
        /// <returns>Les suggestions de solution.</returns>
        Task<IEnumerable<AISuggestion>> SuggestSolutionsAsync(Guid logId, string? sourceCode = null);

        /// <summary>
        /// Génère un rapport d'analyse pour un service.
        /// </summary>
        /// <param name="serviceId">L'identifiant du service.</param>
        /// <param name="startDate">Date de début de l'analyse.</param>
        /// <param name="endDate">Date de fin de l'analyse.</param>
        /// <returns>Le rapport d'analyse.</returns>
        Task<AIAnalysisReport> GenerateReportAsync(Guid serviceId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Catégorise automatiquement un log.
        /// </summary>
        /// <param name="logEntry">L'entrée de log à catégoriser.</param>
        /// <returns>Les catégories suggérées.</returns>
        Task<IEnumerable<string>> CategorizeLogAsync(LogEntry logEntry);

        /// <summary>
        /// Exécute un workflow n8n pour l'analyse d'un log.
        /// </summary>
        /// <param name="logId">L'identifiant du log.</param>
        /// <param name="workflowName">Le nom du workflow à exécuter.</param>
        /// <param name="parameters">Les paramètres additionnels pour le workflow.</param>
        /// <returns>Le résultat de l'exécution du workflow.</returns>
        Task<AIWorkflowResult> ExecuteWorkflowAsync(Guid logId, string workflowName, Dictionary<string, object>? parameters = null);
    }

    /// <summary>
    /// Résultat d'une analyse IA.
    /// </summary>
    public class AIAnalysisResult
    {
        /// <summary>
        /// Identifiant unique de l'analyse.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date et heure de l'analyse.
        /// </summary>
        public DateTime AnalyzedAt { get; set; }

        /// <summary>
        /// Résumé de l'analyse.
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Niveau de confiance de l'analyse (0-100).
        /// </summary>
        public int ConfidenceLevel { get; set; }

        /// <summary>
        /// Anomalies détectées.
        /// </summary>
        public List<AIAnomaly> Anomalies { get; set; } = new List<AIAnomaly>();

        /// <summary>
        /// Suggestions proposées.
        /// </summary>
        public List<AISuggestion> Suggestions { get; set; } = new List<AISuggestion>();

        /// <summary>
        /// Données brutes de l'analyse au format JSON.
        /// </summary>
        public string? RawData { get; set; }
    }

    /// <summary>
    /// Anomalie détectée par l'IA.
    /// </summary>
    public class AIAnomaly
    {
        /// <summary>
        /// Identifiant unique de l'anomalie.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Type d'anomalie.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Description de l'anomalie.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Niveau de gravité de l'anomalie.
        /// </summary>
        public LogLevel Severity { get; set; }

        /// <summary>
        /// Identifiants des logs liés à cette anomalie.
        /// </summary>
        public List<Guid> RelatedLogIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Fréquence d'occurrence de l'anomalie.
        /// </summary>
        public int OccurrenceCount { get; set; }

        /// <summary>
        /// Date et heure de la première occurrence.
        /// </summary>
        public DateTime FirstOccurrence { get; set; }

        /// <summary>
        /// Date et heure de la dernière occurrence.
        /// </summary>
        public DateTime LastOccurrence { get; set; }
    }

    /// <summary>
    /// Suggestion proposée par l'IA.
    /// </summary>
    public class AISuggestion
    {
        /// <summary>
        /// Identifiant unique de la suggestion.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Titre de la suggestion.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description détaillée de la suggestion.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Code ou actions à exécuter pour appliquer la suggestion.
        /// </summary>
        public string? ActionCode { get; set; }

        /// <summary>
        /// Type de suggestion (correction de code, optimisation, configuration, etc.).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Niveau de confiance de la suggestion (0-100).
        /// </summary>
        public int ConfidenceLevel { get; set; }

        /// <summary>
        /// Références ou ressources supplémentaires pour la suggestion.
        /// </summary>
        public List<string> References { get; set; } = new List<string>();
    }

    /// <summary>
    /// Rapport d'analyse IA.
    /// </summary>
    public class AIAnalysisReport
    {
        /// <summary>
        /// Identifiant unique du rapport.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identifiant du service analysé.
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Date et heure de génération du rapport.
        /// </summary>
        public DateTime GeneratedAt { get; set; }

        /// <summary>
        /// Période couverte par le rapport.
        /// </summary>
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Résumé exécutif du rapport.
        /// </summary>
        public string ExecutiveSummary { get; set; } = string.Empty;

        /// <summary>
        /// Statistiques globales des logs.
        /// </summary>
        public Dictionary<LogLevel, int> LogLevelStats { get; set; } = new Dictionary<LogLevel, int>();

        /// <summary>
        /// Anomalies détectées.
        /// </summary>
        public List<AIAnomaly> Anomalies { get; set; } = new List<AIAnomaly>();

        /// <summary>
        /// Suggestions d'amélioration.
        /// </summary>
        public List<AISuggestion> Suggestions { get; set; } = new List<AISuggestion>();

        /// <summary>
        /// Tendances identifiées dans les logs.
        /// </summary>
        public List<string> Trends { get; set; } = new List<string>();

        /// <summary>
        /// Analyse des performances du service.
        /// </summary>
        public string PerformanceAnalysis { get; set; } = string.Empty;

        /// <summary>
        /// Contenu HTML du rapport pour l'affichage.
        /// </summary>
        public string? HtmlContent { get; set; }
    }

    /// <summary>
    /// Résultat de l'exécution d'un workflow n8n.
    /// </summary>
    public class AIWorkflowResult
    {
        /// <summary>
        /// Identifiant unique du résultat.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du workflow exécuté.
        /// </summary>
        public string WorkflowName { get; set; } = string.Empty;

        /// <summary>
        /// Date et heure d'exécution.
        /// </summary>
        public DateTime ExecutedAt { get; set; }

        /// <summary>
        /// Statut de l'exécution.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Résultat de l'exécution au format JSON.
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// Erreur survenue pendant l'exécution, le cas échéant.
        /// </summary>
        public string? Error { get; set; }
    }
}