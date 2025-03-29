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
    /// Contrôleur pour la gestion des services.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<ServicesController> _logger;

        /// <summary>
        /// Constructeur du contrôleur de services.
        /// </summary>
        public ServicesController(
            IServiceRepository serviceRepository,
            IClientRepository clientRepository,
            IAuthService authService,
            ILogger<ServicesController> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Récupère tous les services.
        /// </summary>
        /// <param name="includeInactive">Indique si les services inactifs doivent être inclus.</param>
        /// <returns>La liste des services.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Support")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllServices([FromQuery] bool includeInactive = false)
        {
            try
            {
                var services = await _serviceRepository.GetAllAsync(includeInactive);
                var serviceDtos = services.Select(MapServiceToDto).ToList();
                return Ok(serviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des services");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la récupération des services." });
            }
        }

        /// <summary>
        /// Récupère un service par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du service.</param>
        /// <returns>Le service demandé.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new { message = "Service non trouvé." });
                }

                // Vérification des droits d'accès
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasServiceAccessAsync(userId, id);
                if (!hasAccess)
                {
                    return Forbid();
                }

                var serviceDto = MapServiceToDto(service);
                return Ok(serviceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du service {ServiceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la récupération du service." });
            }
        }

        /// <summary>
        /// Crée un nouveau service.
        /// </summary>
        /// <param name="request">Les données du service à créer.</param>
        /// <returns>Le service créé.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Le corps de la requête est vide." });
            }

            try
            {
                // Vérification de l'existence du client
                var client = await _clientRepository.GetByIdAsync(request.ClientId);
                if (client == null)
                {
                    return BadRequest(new { message = "Le client spécifié n'existe pas." });
                }

                // Vérification des droits d'accès au client
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasClientAccessAsync(userId, request.ClientId);
                if (!hasAccess)
                {
                    return Forbid();
                }

                // Génération d'une clé API unique
                var apiKey = Guid.NewGuid().ToString("N") + "-" + Guid.NewGuid().ToString("N");

                // Création du service
                var service = new RegisteredService
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Version = request.Version,
                    ServiceType = request.ServiceType,
                    ApiKey = apiKey,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow,
                    ClientId = request.ClientId,
                    ClientName = client.Name,
                    Environment = request.Environment,
                    ReportingIntervalMinutes = request.ReportingIntervalMinutes,
                    IsActive = true,
                    IsOnline = false,
                    AlertsEnabled = request.AlertsEnabled,
                    AlertThreshold = request.AlertThreshold,
                    AlertEmailRecipients = request.AlertEmailRecipients,
                    WebhookUrl = request.WebhookUrl,
                    Metadata = request.Metadata,
                    SourceCodePath = request.SourceCodePath
                };

                var createdService = await _serviceRepository.AddAsync(service);
                var serviceDto = MapServiceToDto(createdService);

                return CreatedAtAction(nameof(GetServiceById), new { id = serviceDto.Id }, serviceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du service");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la création du service." });
            }
        }

        /// <summary>
        /// Met à jour un service existant.
        /// </summary>
        /// <param name="id">Identifiant du service à mettre à jour.</param>
        /// <param name="request">Les nouvelles données du service.</param>
        /// <returns>Le service mis à jour.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpdateServiceRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Le corps de la requête est vide." });
            }

            try
            {
                // Récupération du service existant
                var service = await _serviceRepository.GetByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new { message = "Service non trouvé." });
                }

                // Vérification des droits d'accès
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasServiceAccessAsync(userId, id);
                if (!hasAccess)
                {
                    return Forbid();
                }

                // Mise à jour des champs modifiables
                if (request.Name != null) service.Name = request.Name;
                if (request.Description != null) service.Description = request.Description;
                if (request.Version != null) service.Version = request.Version;
                if (request.ServiceType != null) service.ServiceType = request.ServiceType;
                if (request.Environment != null) service.Environment = request.Environment;
                if (request.ReportingIntervalMinutes.HasValue) service.ReportingIntervalMinutes = request.ReportingIntervalMinutes.Value;
                if (request.AlertsEnabled.HasValue) service.AlertsEnabled = request.AlertsEnabled.Value;
                if (request.AlertThreshold.HasValue) service.AlertThreshold = request.AlertThreshold.Value;
                if (request.AlertEmailRecipients != null) service.AlertEmailRecipients = request.AlertEmailRecipients;
                if (request.WebhookUrl != null) service.WebhookUrl = request.WebhookUrl;
                if (request.Metadata != null) service.Metadata = request.Metadata;
                if (request.SourceCodePath != null) service.SourceCodePath = request.SourceCodePath;

                service.LastUpdatedAt = DateTime.UtcNow;

                // Sauvegarde des modifications
                var updated = await _serviceRepository.UpdateAsync(service);
                if (!updated)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "La mise à jour du service a échoué." });
                }

                var serviceDto = MapServiceToDto(service);
                return Ok(serviceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du service {ServiceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la mise à jour du service." });
            }
        }

        /// <summary>
        /// Active un service.
        /// </summary>
        /// <param name="id">Identifiant du service à activer.</param>
        /// <returns>Statut d'activation.</returns>
        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActivateService(Guid id)
        {
            try
            {
                // Vérification de l'existence du service
                var service = await _serviceRepository.GetByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new { message = "Service non trouvé." });
                }

                // Vérification des droits d'accès
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasServiceAccessAsync(userId, id);
                if (!hasAccess)
                {
                    return Forbid();
                }

                // Activation du service
                var activated = await _serviceRepository.ActivateAsync(id);
                if (!activated)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "L'activation du service a échoué." });
                }

                return Ok(new { message = "Service activé avec succès." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'activation du service {ServiceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de l'activation du service." });
            }
        }

        /// <summary>
        /// Désactive un service.
        /// </summary>
        /// <param name="id">Identifiant du service à désactiver.</param>
        /// <returns>Statut de désactivation.</returns>
        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeactivateService(Guid id)
        {
            try
            {
                // Vérification de l'existence du service
                var service = await _serviceRepository.GetByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new { message = "Service non trouvé." });
                }

                // Vérification des droits d'accès
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasServiceAccessAsync(userId, id);
                if (!hasAccess)
                {
                    return Forbid();
                }

                // Désactivation du service
                var deactivated = await _serviceRepository.DeactivateAsync(id);
                if (!deactivated)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "La désactivation du service a échoué." });
                }

                return Ok(new { message = "Service désactivé avec succès." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la désactivation du service {ServiceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la désactivation du service." });
            }
        }

        /// <summary>
        /// Régénère la clé API d'un service.
        /// </summary>
        /// <param name="id">Identifiant du service.</param>
        /// <returns>La nouvelle clé API.</returns>
        [HttpPost("{id}/regenerate-api-key")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(RegenerateApiKeyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegenerateApiKey(Guid id)
        {
            try
            {
                // Vérification de l'existence du service
                var service = await _serviceRepository.GetByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new { message = "Service non trouvé." });
                }

                // Vérification des droits d'accès
                var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? string.Empty);
                var hasAccess = await _authService.HasServiceAccessAsync(userId, id);
                if (!hasAccess)
                {
                    return Forbid();
                }

                // Régénération de la clé API
                var newApiKey = await _serviceRepository.RegenerateApiKeyAsync(id);
                if (newApiKey == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new RegenerateApiKeyResponse
                    {
                        Success = false,
                        ServiceId = id,
                        ErrorMessage = "La régénération de la clé API a échoué."
                    });
                }

                return Ok(new RegenerateApiKeyResponse
                {
                    ApiKey = newApiKey,
                    ServiceId = id,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la régénération de la clé API du service {ServiceId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new RegenerateApiKeyResponse
                {
                    Success = false,
                    ServiceId = id,
                    ErrorMessage = "Une erreur est survenue lors de la régénération de la clé API."
                });
            }
        }

        /// <summary>
        /// Recherche des services selon divers critères.
        /// </summary>
        /// <param name="request">Critères de recherche.</param>
        /// <returns>Les services correspondant aux critères.</returns>
        [HttpPost("search")]
        [Authorize(Roles = "Admin,Support")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchServices([FromBody] SearchServicesRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Le corps de la requête est vide." });
            }

            try
            {
                IEnumerable<RegisteredService> services;
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    services = await _serviceRepository.SearchAsync(request.SearchTerm, request.ClientId);
                }
                else if (request.ClientId.HasValue)
                {
                    services = await _serviceRepository.GetByClientIdAsync(request.ClientId.Value);
                }
                else
                {
                    services = await _serviceRepository.GetAllAsync(request.IncludeInactive);
                }

                var serviceDtos = services.Select(MapServiceToDto).ToList();
                return Ok(serviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de services");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la recherche des services." });
            }
        }

        /// <summary>
        /// Récupère les services sans logs récents.
        /// </summary>
        /// <returns>Les services sans activité récente.</returns>
        [HttpGet("offline")]
        [Authorize(Roles = "Admin,Support")]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOfflineServices()
        {
            try
            {
                var services = await _serviceRepository.GetServicesWithoutRecentLogsAsync();
                var serviceDtos = services.Select(MapServiceToDto).ToList();
                return Ok(serviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des services hors ligne");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur est survenue lors de la récupération des services hors ligne." });
            }
        }

        /// <summary>
        /// Mappe une entité RegisteredService vers un DTO ServiceDto.
        /// </summary>
        /// <param name="service">L'entité à mapper.</param>
        /// <returns>Le DTO résultant.</returns>
        private static ServiceDto MapServiceToDto(RegisteredService service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Version = service.Version,
                ServiceType = service.ServiceType,
                ApiKey = service.ApiKey,
                CreatedAt = service.CreatedAt,
                LastUpdatedAt = service.LastUpdatedAt,
                LastLogReceivedAt = service.LastLogReceivedAt,
                ClientId = service.ClientId,
                ClientName = service.ClientName,
                Environment = service.Environment,
                ReportingIntervalMinutes = service.ReportingIntervalMinutes,
                IsActive = service.IsActive,
                IsOnline = service.IsOnline,
                AlertsEnabled = service.AlertsEnabled,
                AlertThreshold = service.AlertThreshold,
                AlertEmailRecipients = service.AlertEmailRecipients,
                WebhookUrl = service.WebhookUrl,
                Metadata = service.Metadata,
                SourceCodePath = service.SourceCodePath
            };
        }
    }
}