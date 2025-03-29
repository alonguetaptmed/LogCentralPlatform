using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Core.Interfaces
{
    /// <summary>
    /// Interface pour le dépôt des clients.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Ajoute un nouveau client.
        /// </summary>
        /// <param name="client">Le client à ajouter.</param>
        /// <returns>Le client créé avec son identifiant.</returns>
        Task<Client> AddAsync(Client client);
        
        /// <summary>
        /// Met à jour un client existant.
        /// </summary>
        /// <param name="client">Les nouvelles données du client.</param>
        /// <returns>True si la mise à jour a réussi, false sinon.</returns>
        Task<bool> UpdateAsync(Client client);
        
        /// <summary>
        /// Récupère un client par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du client.</param>
        /// <returns>Le client s'il est trouvé, null sinon.</returns>
        Task<Client?> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Récupère un client par son numéro client.
        /// </summary>
        /// <param name="clientNumber">Le numéro du client.</param>
        /// <returns>Le client s'il est trouvé, null sinon.</returns>
        Task<Client?> GetByClientNumberAsync(string clientNumber);
        
        /// <summary>
        /// Récupère tous les clients.
        /// </summary>
        /// <param name="includeInactive">Indique si les clients inactifs doivent être inclus.</param>
        /// <returns>La liste de tous les clients.</returns>
        Task<IEnumerable<Client>> GetAllAsync(bool includeInactive = false);
        
        /// <summary>
        /// Recherche des clients par nom ou numéro.
        /// </summary>
        /// <param name="searchTerm">Le terme de recherche.</param>
        /// <returns>La liste des clients correspondant aux critères.</returns>
        Task<IEnumerable<Client>> SearchAsync(string searchTerm);
        
        /// <summary>
        /// Désactive un client.
        /// </summary>
        /// <param name="id">L'identifiant du client.</param>
        /// <returns>True si la désactivation a réussi, false sinon.</returns>
        Task<bool> DeactivateAsync(Guid id);
        
        /// <summary>
        /// Active un client.
        /// </summary>
        /// <param name="id">L'identifiant du client.</param>
        /// <returns>True si l'activation a réussi, false sinon.</returns>
        Task<bool> ActivateAsync(Guid id);
        
        /// <summary>
        /// Ajoute un contact à un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <param name="contact">Le contact à ajouter.</param>
        /// <returns>True si l'ajout a réussi, false sinon.</returns>
        Task<bool> AddContactAsync(Guid clientId, ContactPerson contact);
        
        /// <summary>
        /// Met à jour un contact d'un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <param name="contact">Les nouvelles données du contact.</param>
        /// <returns>True si la mise à jour a réussi, false sinon.</returns>
        Task<bool> UpdateContactAsync(Guid clientId, ContactPerson contact);
        
        /// <summary>
        /// Supprime un contact d'un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <param name="contactId">L'identifiant du contact.</param>
        /// <returns>True si la suppression a réussi, false sinon.</returns>
        Task<bool> RemoveContactAsync(Guid clientId, Guid contactId);
        
        /// <summary>
        /// Met à jour les paramètres de notification d'un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <param name="settings">Les nouveaux paramètres de notification.</param>
        /// <returns>True si la mise à jour a réussi, false sinon.</returns>
        Task<bool> UpdateNotificationSettingsAsync(Guid clientId, NotificationSettings settings);
        
        /// <summary>
        /// Vérifie si un client existe par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du client.</param>
        /// <returns>True si le client existe, false sinon.</returns>
        Task<bool> ExistsAsync(Guid id);
        
        /// <summary>
        /// Vérifie si un numéro client est déjà utilisé.
        /// </summary>
        /// <param name="clientNumber">Le numéro client à vérifier.</param>
        /// <param name="excludeClientId">Identifiant du client à exclure de la vérification (optionnel).</param>
        /// <returns>True si le numéro est déjà utilisé, false sinon.</returns>
        Task<bool> IsClientNumberUsedAsync(string clientNumber, Guid? excludeClientId = null);
    }
}