using LogCentralPlatform.Core.Entities;
using LogCentralPlatform.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LogCentralPlatform.Infrastructure.Services
{
    /// <summary>
    /// Implémentation du service d'analyse IA des logs utilisant n8n.
    /// </summary>
    public class AIAnalysisService : IAIAnalysisService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AIAnalysisService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _n8nApiUrl;
        private readonly string _n8nApiKey;

        /// <summary>
        /// Constructeur du service d'analyse IA.
        /// </summary>
        /// <param name="configuration">Configuration de l'application.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="httpClientFactory">Factory pour HttpClient.</param>
        public AIAnalysisService(
            IConfiguration configuration,
            ILogger<AIAnalysisService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClientFactory?.CreateClient("N8nClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
            
            // Récupération des paramètres de configuration
            _n8nApiUrl = _configuration["AISettings:N8nApiUrl"] ?? throw new InvalidOperationException("La configuration N8nApiUrl est manquante");
            _n8nApiKey = _configuration["AISettings:N8nApiKey"] ?? throw new InvalidOperationException("La configuration N8nApiKey est manquante");
            
            // Configuration de l'HttpClient pour n8n
            _httpClient.DefaultRequestHeaders.Add("X-N8N-API-KEY", _n8nApiKey);
        }

        /// <inheritdoc/>
        public async Task<AIAnalysisResult> AnalyzeLogAsync(LogEntry logEntry)
        {
            try
            {
                _logger.LogInformation("Démarrage de l'analyse IA pour le log {LogId}", logEntry.Id);

                // Création d'une analyse simple pour le moment (sans appel à n8n)
                // Dans une implémentation réelle, ce code enverrait les données à n8n
                var analysisResult = new AIAnalysisResult
                {
                    Id = Guid.NewGuid(),
                    AnalyzedAt = DateTime.UtcNow,
                    Summary = GenerateBasicAnalysis(logEntry),
                    ConfidenceLevel = 80,
                    Anomalies = new List<AIAnomaly>(),
                    Suggestions = new List<AISuggestion>()
                };

                // Ajout d'anomalies et suggestions simples basées sur le niveau de log
                if (logEntry.Level >= LogLevel.Error)
                {
                    analysisResult.Anomalies.Add(new AIAnomaly
                    {
                        Id = Guid.NewGuid(),
                        Type = logEntry.Level == LogLevel.Error ? "Error" : "Critical Error",
                        Description = $"Detected {logEntry.Level} in service {logEntry.ServiceName}: {logEntry.Message}",
                        Severity = logEntry.Level,
                        RelatedLogIds = new List<Guid> { logEntry.Id },
                        OccurrenceCount = 1,
                        FirstOccurrence = logEntry.Timestamp,
                        LastOccurrence = logEntry.Timestamp
                    });

                    analysisResult.Suggestions.Add(new AISuggestion
                    {
                        Id = Guid.NewGuid(),
                        Title = "Investigate Error Source",
                        Description = $"Investigate the root cause of the {logEntry.Level} in {logEntry.ServiceName}. " +
                                      $"Check the error message and stack trace for details.",
                        Type = "Troubleshooting",
                        ConfidenceLevel = 85,
                        References = new List<string> { "Error logs", "Service documentation" }
                    });
                }

                // Simuler l'appel API à n8n (à implémenter dans une version future)
                // await CallN8nWorkflowAsync("analyze-log", logEntry);

                return analysisResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'analyse IA du log {LogId}", logEntry.Id);
                
                // Retourner un résultat par défaut en cas d'erreur
                return new AIAnalysisResult
                {
                    Id = Guid.NewGuid(),
                    AnalyzedAt = DateTime.UtcNow,
                    Summary = $"AI analysis failed: {ex.Message}",
                    ConfidenceLevel = 0,
                    Anomalies = new List<AIAnomaly>(),
                    Suggestions = new List<AISuggestion>()
                };
            }
        }

        /// <inheritdoc/>
        public async Task<AIAnalysisResult> AnalyzeLogsPatternAsync(IEnumerable<LogEntry> logs)
        {
            try
            {
                _logger.LogInformation("Démarrage de l'analyse de motifs sur un ensemble de logs");

                // Implementation simplifiée pour la première version
                // Dans une implémentation réelle, ce code enverrait les données à n8n
                var analysisResult = new AIAnalysisResult
                {
                    Id = Guid.NewGuid(),
                    AnalyzedAt = DateTime.UtcNow,
                    Summary = "Pattern analysis of multiple logs",
                    ConfidenceLevel = 75,
                    Anomalies = new List<AIAnomaly>(),
                    Suggestions = new List<AISuggestion>()
                };

                // Analyser les motifs de base
                var logCount = logs.Count();
                var errorCount = logs.Count(l => l.Level >= LogLevel.Error);
                var warningCount = logs.Count(l => l.Level == LogLevel.Warning);
                var serviceGroups = logs.GroupBy(l => l.ServiceId).ToList();

                // Enrichir le résumé
                analysisResult.Summary = $"Analyzed {logCount} logs across {serviceGroups.Count} services. " +
                                         $"Found {errorCount} errors and {warningCount} warnings.";

                // Détection de motifs simples
                if (errorCount > 0)
                {
                    // Regrouper les erreurs similaires
                    var errorGroups = logs
                        .Where(l => l.Level >= LogLevel.Error)
                        .GroupBy(l => new { l.Message })
                        .Where(g => g.Count() > 1)
                        .OrderByDescending(g => g.Count())
                        .Take(5)
                        .ToList();

                    foreach (var group in errorGroups)
                    {
                        var logs = group.ToList();
                        analysisResult.Anomalies.Add(new AIAnomaly
                        {
                            Id = Guid.NewGuid(),
                            Type = "Recurring Error",
                            Description = $"Recurring error: {group.Key.Message}",
                            Severity = LogLevel.Error,
                            RelatedLogIds = logs.Select(l => l.Id).ToList(),
                            OccurrenceCount = logs.Count,
                            FirstOccurrence = logs.Min(l => l.Timestamp),
                            LastOccurrence = logs.Max(l => l.Timestamp)
                        });
                    }

                    // Ajouter une suggestion générale
                    analysisResult.Suggestions.Add(new AISuggestion
                    {
                        Id = Guid.NewGuid(),
                        Title = "Error Pattern Investigation",
                        Description = "Multiple similar errors detected. Consider investigating the pattern to identify common root causes.",
                        Type = "Pattern Analysis",
                        ConfidenceLevel = 80,
                        References = new List<string> { "Error logs", "Service patterns" }
                    });
                }

                return analysisResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'analyse de motifs sur un ensemble de logs");
                
                // Retourner un résultat par défaut en cas d'erreur
                return new AIAnalysisResult
                {
                    Id = Guid.NewGuid(),
                    AnalyzedAt = DateTime.UtcNow,
                    Summary = $"Pattern analysis failed: {ex.Message}",
                    ConfidenceLevel = 0,
                    Anomalies = new List<AIAnomaly>(),
                    Suggestions = new List<AISuggestion>()
                };
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AIAnomaly>> DetectAnomaliesAsync(Guid serviceId, DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Détection d'anomalies pour le service {ServiceId} entre {StartDate} et {EndDate}", 
                    serviceId, startDate, endDate);

                // Implémentation simplifiée
                // Dans une implémentation réelle, ce code ferait appel à un algorithme de détection d'anomalies
                var anomalies = new List<AIAnomaly>();
                
                // Simuler la détection d'une anomalie
                anomalies.Add(new AIAnomaly
                {
                    Id = Guid.NewGuid(),
                    Type = "Unusual Error Rate",
                    Description = $"Unusual error rate detected for service {serviceId} between {startDate} and {endDate}",
                    Severity = LogLevel.Warning,
                    RelatedLogIds = new List<Guid>(),
                    OccurrenceCount = 1,
                    FirstOccurrence = startDate,
                    LastOccurrence = endDate
                });

                return anomalies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la détection d'anomalies pour le service {ServiceId}", serviceId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AISuggestion>> SuggestSolutionsAsync(Guid logId, string? sourceCode = null)
        {
            try
            {
                _logger.LogInformation("Suggestion de solutions pour le log {LogId}", logId);

                // Implémentation simplifiée
                // Dans une implémentation réelle, ce code ferait appel à un LLM ou à n8n
                var suggestions = new List<AISuggestion>();

                // Simuler une suggestion
                suggestions.Add(new AISuggestion
                {
                    Id = Guid.NewGuid(),
                    Title = "Generic Troubleshooting",
                    Description = "This is a placeholder suggestion. In a real implementation, " +
                                 "the AI would analyze the log and source code to provide actionable solutions.",
                    Type = "Troubleshooting",
                    ConfidenceLevel = 70,
                    References = new List<string> { "Best practices", "Common error patterns" }
                });

                if (!string.IsNullOrEmpty(sourceCode))
                {
                    suggestions.Add(new AISuggestion
                    {
                        Id = Guid.NewGuid(),
                        Title = "Source Code Review",
                        Description = "Source code has been provided. In a real implementation, " +
                                     "the AI would analyze the code to identify potential issues.",
                        Type = "Code Analysis",
                        ConfidenceLevel = 75,
                        References = new List<string> { "Code best practices", "Static analysis" }
                    });
                }

                return suggestions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suggestion de solutions pour le log {LogId}", logId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<AIAnalysisReport> GenerateReportAsync(Guid serviceId, DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Génération d'un rapport d'analyse pour le service {ServiceId} entre {StartDate} et {EndDate}", 
                    serviceId, startDate, endDate);

                // Implémentation simplifiée
                // Dans une implémentation réelle, ce code ferait appel à n8n et à un LLM
                var report = new AIAnalysisReport
                {
                    Id = Guid.NewGuid(),
                    ServiceId = serviceId,
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = startDate,
                    EndDate = endDate,
                    ExecutiveSummary = $"This is a report for service {serviceId} covering the period from {startDate} to {endDate}.",
                    LogLevelStats = new Dictionary<LogLevel, int>
                    {
                        { LogLevel.Information, 100 },
                        { LogLevel.Warning, 20 },
                        { LogLevel.Error, 5 },
                        { LogLevel.Critical, 1 }
                    },
                    Anomalies = new List<AIAnomaly>(),
                    Suggestions = new List<AISuggestion>(),
                    Trends = new List<string>
                    {
                        "Error rate has been stable",
                        "Service performance is consistent"
                    },
                    PerformanceAnalysis = "Performance has been consistent throughout the period. No significant degradation observed.",
                    HtmlContent = $"<h1>Service Analysis Report</h1><p>Period: {startDate} to {endDate}</p>"
                };

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la génération du rapport pour le service {ServiceId}", serviceId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> CategorizeLogAsync(LogEntry logEntry)
        {
            try
            {
                _logger.LogInformation("Catégorisation du log {LogId}", logEntry.Id);

                // Implémentation simplifiée
                // Dans une implémentation réelle, ce code ferait appel à un modèle de classification
                var categories = new List<string>();

                // Catégorisation basique selon le niveau du log
                if (logEntry.Level >= LogLevel.Error)
                {
                    categories.Add("Error");
                    if (logEntry.Level == LogLevel.Critical)
                    {
                        categories.Add("Critical");
                    }
                }
                else if (logEntry.Level == LogLevel.Warning)
                {
                    categories.Add("Warning");
                }
                else
                {
                    categories.Add("Information");
                }

                // Catégorisation selon le contenu du message
                if (logEntry.Message.Contains("database", StringComparison.OrdinalIgnoreCase))
                {
                    categories.Add("Database");
                }
                else if (logEntry.Message.Contains("network", StringComparison.OrdinalIgnoreCase))
                {
                    categories.Add("Network");
                }
                else if (logEntry.Message.Contains("auth", StringComparison.OrdinalIgnoreCase))
                {
                    categories.Add("Authentication");
                }

                // Ajouter la catégorie explicite si elle existe
                if (!string.IsNullOrEmpty(logEntry.Category))
                {
                    categories.Add(logEntry.Category);
                }

                return categories.Distinct();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la catégorisation du log {LogId}", logEntry.Id);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<AIWorkflowResult> ExecuteWorkflowAsync(Guid logId, string workflowName, Dictionary<string, object>? parameters = null)
        {
            try
            {
                _logger.LogInformation("Exécution du workflow {WorkflowName} pour le log {LogId}", workflowName, logId);

                // Simulation d'un appel à n8n
                // Dans une implémentation réelle, ce code enverrait une requête HTTP à n8n
                await Task.Delay(500);

                var result = new AIWorkflowResult
                {
                    Id = Guid.NewGuid(),
                    WorkflowName = workflowName,
                    ExecutedAt = DateTime.UtcNow,
                    Status = "Completed",
                    Result = "{ \"success\": true, \"message\": \"Workflow executed successfully\" }"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'exécution du workflow {WorkflowName} pour le log {LogId}", workflowName, logId);
                
                return new AIWorkflowResult
                {
                    Id = Guid.NewGuid(),
                    WorkflowName = workflowName,
                    ExecutedAt = DateTime.UtcNow,
                    Status = "Failed",
                    Error = ex.Message
                };
            }
        }

        #region Private Methods

        /// <summary>
        /// Génère une analyse basique d'un log.
        /// </summary>
        /// <param name="logEntry">L'entrée de log à analyser.</param>
        /// <returns>Un résumé de l'analyse.</returns>
        private string GenerateBasicAnalysis(LogEntry logEntry)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Log Analysis for ID: {logEntry.Id}");
            builder.AppendLine($"Service: {logEntry.ServiceName} (v{logEntry.ServiceVersion})");
            builder.AppendLine($"Environment: {logEntry.Environment}");
            builder.AppendLine($"Timestamp: {logEntry.Timestamp}");
            builder.AppendLine($"Level: {logEntry.Level}");
            builder.AppendLine();
            
            // Analyse selon le niveau de log
            switch (logEntry.Level)
            {
                case LogLevel.Critical:
                    builder.AppendLine("This is a CRITICAL error that requires immediate attention.");
                    break;
                case LogLevel.Error:
                    builder.AppendLine("This is an ERROR that should be investigated.");
                    break;
                case LogLevel.Warning:
                    builder.AppendLine("This is a WARNING that may indicate potential issues.");
                    break;
                default:
                    builder.AppendLine("This is an informational log that doesn't require immediate action.");
                    break;
            }
            
            // Analyser le message et la stack trace si disponible
            builder.AppendLine();
            builder.AppendLine("Message Analysis:");
            builder.AppendLine(logEntry.Message);
            
            if (!string.IsNullOrEmpty(logEntry.ExceptionDetails))
            {
                builder.AppendLine();
                builder.AppendLine("Exception Analysis:");
                builder.AppendLine(logEntry.ExceptionDetails);
            }
            
            return builder.ToString();
        }

        /// <summary>
        /// Appelle un workflow n8n.
        /// </summary>
        /// <param name="workflowName">Nom du workflow.</param>
        /// <param name="data">Données à envoyer.</param>
        /// <returns>Le résultat du workflow.</returns>
        private async Task<string> CallN8nWorkflowAsync(string workflowName, object data)
        {
            try
            {
                var url = $"{_n8nApiUrl}/{workflowName}";
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'appel au workflow n8n {WorkflowName}", workflowName);
                throw;
            }
        }
        
        #endregion
    }
}