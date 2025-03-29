using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogCentralPlatform.Api.Models;
using LogCentralPlatform.Core.Entities;
using LogCentralPlatform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogCentralPlatform.Api.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des logs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IAuthService _authService;
        private readonly IAIAnalysisService _aiAnalysisService;
        private readonly ILogger<LogsController> _logger;

        /// <summary>
        /// Constructeur du contrôleur de logs.
        /// </summary>
        public LogsController(
            ILogRepository logRepository,
            IServiceRepository serviceRepository,
            IAuthService authService,
            IAIAnalysisService aiAnalysisService,
            ILogger<LogsController> logger)
        {
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _aiAnalysisService = aiAnalysisService ?? throw new ArgumentNullException(nameof(aiAnalysisService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Enregistre un nouveau log.
        /// </summary>
        /// <param name="createLogRequest">Les données du log à créer.</param>
        /// <param name="apiKey">Clé API du service.</param>
        /// <returns>Résultat de la création du log.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateLogResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateLog(
            [FromBody] CreateLogRequest createLogRequest,
            [FromHeader(Name = "X-API-Key")] string apiKey)
        {
            if (createLogRequest == null)
            {
                return BadRequest(new CreateLogResponse
                {
                    Success = false,
                    ErrorMessage = "Le corps de la requête est vide."
                });
            }

            try
            {
                // Authentification du service par clé API
                var serviceAuth = await _authService.AuthenticateServiceAsync(apiKey);
                if (!serviceAuth.Success || serviceAuth.ServiceId == null)
                {
                    return Unauthorized(new CreateLogResponse
                    {
                        Success = false,
                        ErrorMessage = "Clé API invalide ou non autorisée."
                    });
                }

                // Récupération des informations du service
                var service = await _serviceRepository.GetByIdAsync(serviceAuth.ServiceId.Value);
                if (service == null)
                {
                    return Unauthorized(new CreateLogResponse
                    {
                        Success = false,
                        ErrorMessage = "Service non trouvé."
                    });
                }

                // Mise à jour du statut en ligne du service
                await _serviceRepository.UpdateOnlineStatusAsync(service.Id, true, DateTime.UtcNow);

                // Création de l'entrée de log
                var logEntry = new LogEntry
                {
                    Id = Guid.NewGuid(),
                    Timestamp = createLogRequest.Timestamp,
                    Level = createLogRequest.Level,
                    Message = createLogRequest.Message,
                    ServiceId = service.Id,
                    ServiceName = service.Name,
                    ServiceVersion = service.Version,
                    Environment = service.Environment,
                    Category = createLogRequest.Category ?? string.Empty,
                    ClientId = service.ClientId,
                    ClientName = service.ClientName,
                    ExceptionDetails = createLogRequest.ExceptionDetails,
                    StackTrace = createLogRequest.StackTrace,
                    CorrelationId = createLogRequest.CorrelationId,
                    ContextData = createLogRequest.ContextData,
                    ContainsSensitiveData = createLogRequest.ContainsSensitiveData,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    AnalyzedByAI = false,
                    ReceivedAt = DateTime.UtcNow,
                    Metadata = createLogRequest.Metadata
                };

                // Enregistrement du log
                var createdLog = await _logRepository.AddAsync(logEntry);

                // Lancement d'une analyse AI asynchrone si log d'erreur ou critique
                if (createLogRequest.Level >= Core.Entities.LogLevel.Error)
                {
                    // Lancement en arrière-plan pour ne pas bloquer la réponse API
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var analysisResult = await _aiAnalysisService.AnalyzeLogAsync(createdLog);
                            await _logRepository.UpdateAIAnalysisAsync(createdLog.Id, analysisResult.Summary);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Erreur lors de l'analyse AI du log {LogId}", createdLog.Id);
                        }
                    });
                }

                var response = new CreateLogResponse
                {
                    Id = createdLog.Id,
                    ReceivedAt = createdLog.ReceivedAt,
                    Success = true
                };

                return CreatedAtAction(nameof(GetLog), new { id = createdLog.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création d'un log");
                return StatusCode(StatusCodes.Status500InternalServerError, new CreateLogResponse
                {
                    Success = false,
                    ErrorMessage = "Une erreur est survenue lors de la création du log."
                });
            }
        }

        /// <summary>
        /// Récupère un log spécifique par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du log.</param>
        /// <returns>Le log demandé.</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(LogEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetLog(Guid id)
        {
            try
            {
                var logEntry = await _logRepository.GetByIdAsync(id);
                if (logEntry == null)
                {
                    return NotFound(new { message = "Log non trouvé." });
                }

                // Vérification des droits d'accès
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasServiceAccessAsync(userId, logEntry.ServiceId);
                if (!hasAccess)
                {
                    return Forbid();
                }

                var logDto = MapLogEntryToDto(logEntry);
                return Ok(logDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du log {LogId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la récupération du log." });
            }
        }

        /// <summary>
        /// Recherche des logs selon divers critères.
        /// </summary>
        /// <param name="request">Critères de recherche.</param>
        /// <returns>Les logs correspondant aux critères.</returns>
        [HttpPost("search")]
        [Authorize]
        [ProducesResponseType(typeof(SearchLogsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SearchLogs([FromBody] SearchLogsRequest request)
        {
            if (request == null)
            {
                return BadRequest(new SearchLogsResponse
                {
                    Success = false,
                    ErrorMessage = "Le corps de la requête est vide."
                });
            }

            try
            {
                // Vérification des dates
                var startDate = request.StartDate ?? DateTime.UtcNow.AddDays(-7);
                var endDate = request.EndDate ?? DateTime.UtcNow;

                if (startDate > endDate)
                {
                    return BadRequest(new SearchLogsResponse
                    {
                        Success = false,
                        ErrorMessage = "La date de début doit être antérieure à la date de fin."
                    });
                }

                // Si un texte de recherche est fourni
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var logs = await _logRepository.SearchByTextAsync(
                        request.SearchText,
                        startDate,
                        endDate,
                        request.ServiceId,
                        request.ClientId,
                        request.Skip,
                        request.Take);

                    var totalCount = 0;
                    if (logs.Any())
                    {
                        // Calcul du total pour la pagination
                        if (request.ServiceId.HasValue)
                        {
                            totalCount = await _logRepository.GetCountByServiceIdAsync(request.ServiceId.Value);
                        }
                        else if (request.ClientId.HasValue)
                        {
                            totalCount = await _logRepository.GetCountByClientIdAsync(request.ClientId.Value);
                        }
                    }

                    var response = new SearchLogsResponse
                    {
                        Logs = logs.Select(MapLogEntryToDto).ToList(),
                        TotalCount = totalCount,
                        Success = true
                    };

                    return Ok(response);
                }
                else
                {
                    // Recherche par critères sans texte
                    var logs = await _logRepository.SearchAsync(
                        startDate,
                        endDate,
                        request.ServiceId,
                        request.ClientId,
                        request.MinLevel,
                        request.Skip,
                        request.Take);

                    int totalCount = 0;
                    if (request.MinLevel.HasValue)
                    {
                        totalCount = await _logRepository.GetCountByLevelAsync(request.MinLevel.Value);
                    }
                    else if (request.ServiceId.HasValue)
                    {
                        totalCount = await _logRepository.GetCountByServiceIdAsync(request.ServiceId.Value);
                    }
                    else if (request.ClientId.HasValue)
                    {
                        totalCount = await _logRepository.GetCountByClientIdAsync(request.ClientId.Value);
                    }

                    var response = new SearchLogsResponse
                    {
                        Logs = logs.Select(MapLogEntryToDto).ToList(),
                        TotalCount = totalCount,
                        Success = true
                    };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de logs");
                return StatusCode(StatusCodes.Status500InternalServerError, new SearchLogsResponse
                {
                    Success = false,
                    ErrorMessage = "Une erreur est survenue lors de la recherche des logs."
                });
            }
        }

        /// <summary>
        /// Déclenche une analyse IA sur un log spécifique.
        /// </summary>
        /// <param name="id">Identifiant du log à analyser.</param>
        /// <returns>Résultat de l'analyse IA.</returns>
        [HttpPost("{id}/analyze")]
        [Authorize(Roles = "Admin,Support")]
        [ProducesResponseType(typeof(AIAnalysisResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AnalyzeLog(Guid id)
        {
            try
            {
                var logEntry = await _logRepository.GetByIdAsync(id);
                if (logEntry == null)
                {
                    return NotFound(new { message = "Log non trouvé." });
                }

                // Lancement de l'analyse
                var analysisResult = await _aiAnalysisService.AnalyzeLogAsync(logEntry);

                // Mise à jour du log avec le résultat de l'analyse
                await _logRepository.UpdateAIAnalysisAsync(id, analysisResult.Summary);

                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'analyse IA du log {LogId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de l'analyse IA du log." });
            }
        }

        /// <summary>
        /// Mappe une entité LogEntry vers un DTO LogEntryDto.
        /// </summary>
        /// <param name="entry">L'entité à mapper.</param>
        /// <returns>Le DTO résultant.</returns>
        private static LogEntryDto MapLogEntryToDto(LogEntry entry)
        {
            return new LogEntryDto
            {
                Id = entry.Id,
                Timestamp = entry.Timestamp,
                Level = entry.Level,
                Message = entry.Message,
                ServiceId = entry.ServiceId,
                ServiceName = entry.ServiceName,
                ServiceVersion = entry.ServiceVersion,
                Environment = entry.Environment,
                Category = entry.Category,
                ClientId = entry.ClientId,
                ClientName = entry.ClientName,
                ExceptionDetails = entry.ExceptionDetails,
                StackTrace = entry.StackTrace,
                CorrelationId = entry.CorrelationId,
                ContextData = entry.ContextData,
                IpAddress = entry.IpAddress,
                AnalyzedByAI = entry.AnalyzedByAI,
                AIAnalysisResult = entry.AIAnalysisResult,
                ReceivedAt = entry.ReceivedAt,
                Metadata = entry.Metadata
            };
        }
    }
}