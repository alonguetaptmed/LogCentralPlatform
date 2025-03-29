using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Core.Interfaces
{
    /// <summary>
    /// Interface pour le dépôt des services enregistrés.
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Ajoute un nouveau service.
        /// </summary>
        /// <param name="service">Le service à ajouter.</param>
        /// <returns>Le service créé avec son identifiant.</returns>
        Task<RegisteredService> AddAsync(RegisteredService service);

        /// <summary>
        /// Met à jour un service existant.
        /// </summary>
        /// <param name="service">Les nouvelles données du service.</param>
        /// <returns>True si la mise à jour a réussi, false sinon.</returns>
        Task<bool> UpdateAsync(RegisteredService service);

        /// <summary>
        /// Récupère un service par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du service.</param>
        /// <returns>Le service s'il est trouvé, null sinon.</returns>
        Task<RegisteredService?> GetByIdAsync(Guid id);

        /// <summary>
        /// Récupère un service par sa clé API.
        /// </summary>
        /// <param name="apiKey">La clé API du service.</param>
        /// <returns>Le service s'il est trouvé, null sinon.</returns>
        Task<RegisteredService?> GetByApiKeyAsync(string apiKey);

        /// <summary>
        /// Récupère tous les services d'un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <returns>La liste des services du client.</returns>
        Task<IEnumerable<RegisteredService>> GetByClientIdAsync(Guid clientId);

        /// <summary>
        /// Récupère tous les services.
        /// </summary>
        /// <param name="includeInactive">Indique si les services inactifs doivent être inclus.</param>
        /// <returns>La liste de tous les services.</returns>
        Task<IEnumerable<RegisteredService>> GetAllAsync(bool includeInactive = false);

        /// <summary>
        /// Met à jour le statut en ligne d'un service.
        /// </summary>
        /// <param name="id">L'identifiant du service.</param>
        /// <param name="isOnline">Le nouveau statut en ligne.</param>
        /// <param name="lastLogReceivedAt">La date de réception du dernier log.</param>
        /// <returns>True si la mise à jour a réussi, false sinon.</returns>
        Task<bool> UpdateOnlineStatusAsync(Guid id, bool isOnline, DateTime lastLogReceivedAt);

        /// <summary>
        /// Désactive un service.
        /// </summary>
        /// <param name="id">L'identifiant du service.</param>
        /// <returns>True si la désactivation a réussi, false sinon.</returns>
        Task<bool> DeactivateAsync(Guid id);

        /// <summary>
        /// Active un service.
        /// </summary>
        /// <param name="id">L'identifiant du service.</param>
        /// <returns>True si l'activation a réussi, false sinon.</returns>
        Task<bool> ActivateAsync(Guid id);

        /// <summary>
        /// Génère une nouvelle clé API pour un service.
        /// </summary>
        /// <param name="id">L'identifiant du service.</param>
        /// <returns>La nouvelle clé API ou null si l'opération a échoué.</returns>
        Task<string?> RegenerateApiKeyAsync(Guid id);

        /// <summary>
        /// Recherche des services par nom, description ou type.
        /// </summary>
        /// <param name="searchTerm">Le terme de recherche.</param>
        /// <param name="clientId">L'identifiant du client (optionnel).</param>
        /// <returns>La liste des services correspondant aux critères.</returns>
        Task<IEnumerable<RegisteredService>> SearchAsync(string searchTerm, Guid? clientId = null);

        /// <summary>
        /// Vérifie si un service existe par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du service.</param>
        /// <returns>True si le service existe, false sinon.</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Récupère les services pour lesquels aucun log n'a été reçu depuis l'intervalle spécifié.
        /// </summary>
        /// <returns>La liste des services n'ayant pas envoyé de logs récemment.</returns>
        Task<IEnumerable<RegisteredService>> GetServicesWithoutRecentLogsAsync();
    }
}