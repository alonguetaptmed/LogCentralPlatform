using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Core.Interfaces
{
    /// <summary>
    /// Interface pour le dépôt des logs.
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Ajoute une entrée de log.
        /// </summary>
        /// <param name="logEntry">L'entrée de log à ajouter.</param>
        /// <returns>L'entrée de log créée avec son identifiant.</returns>
        Task<LogEntry> AddAsync(LogEntry logEntry);

        /// <summary>
        /// Récupère une entrée de log par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'entrée de log.</param>
        /// <returns>L'entrée de log si trouvée, null sinon.</returns>
        Task<LogEntry?> GetByIdAsync(Guid id);

        /// <summary>
        /// Récupère les logs pour un service spécifique.
        /// </summary>
        /// <param name="serviceId">L'identifiant du service.</param>
        /// <param name="skip">Nombre d'éléments à sauter (pagination).</param>
        /// <param name="take">Nombre d'éléments à retourner (pagination).</param>
        /// <returns>La liste des entrées de log du service.</returns>
        Task<IEnumerable<LogEntry>> GetByServiceIdAsync(Guid serviceId, int skip = 0, int take = 100);

        /// <summary>
        /// Récupère les logs pour un client spécifique.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <param name="skip">Nombre d'éléments à sauter (pagination).</param>
        /// <param name="take">Nombre d'éléments à retourner (pagination).</param>
        /// <returns>La liste des entrées de log du client.</returns>
        Task<IEnumerable<LogEntry>> GetByClientIdAsync(Guid clientId, int skip = 0, int take = 100);

        /// <summary>
        /// Récupère les logs par niveau de gravité.
        /// </summary>
        /// <param name="level">Le niveau de gravité.</param>
        /// <param name="skip">Nombre d'éléments à sauter (pagination).</param>
        /// <param name="take">Nombre d'éléments à retourner (pagination).</param>
        /// <returns>La liste des entrées de log du niveau spécifié.</returns>
        Task<IEnumerable<LogEntry>> GetByLevelAsync(LogLevel level, int skip = 0, int take = 100);

        /// <summary>
        /// Recherche des logs sur une période donnée.
        /// </summary>
        /// <param name="startDate">Date de début.</param>
        /// <param name="endDate">Date de fin.</param>
        /// <param name="serviceId">Identifiant du service (optionnel).</param>
        /// <param name="clientId">Identifiant du client (optionnel).</param>
        /// <param name="level">Niveau de gravité minimum (optionnel).</param>
        /// <param name="skip">Nombre d'éléments à sauter (pagination).</param>
        /// <param name="take">Nombre d'éléments à retourner (pagination).</param>
        /// <returns>La liste des entrées de log correspondant aux critères.</returns>
        Task<IEnumerable<LogEntry>> SearchAsync(
            DateTime startDate, 
            DateTime endDate, 
            Guid? serviceId = null, 
            Guid? clientId = null, 
            LogLevel? level = null, 
            int skip = 0, 
            int take = 100);

        /// <summary>
        /// Recherche des logs par texte.
        /// </summary>
        /// <param name="searchText">Texte à rechercher.</param>
        /// <param name="startDate">Date de début (optionnel).</param>
        /// <param name="endDate">Date de fin (optionnel).</param>
        /// <param name="serviceId">Identifiant du service (optionnel).</param>
        /// <param name="clientId">Identifiant du client (optionnel).</param>
        /// <param name="skip">Nombre d'éléments à sauter (pagination).</param>
        /// <param name="take">Nombre d'éléments à retourner (pagination).</param>
        /// <returns>La liste des entrées de log correspondant aux critères.</returns>
        Task<IEnumerable<LogEntry>> SearchByTextAsync(
            string searchText, 
            DateTime? startDate = null, 
            DateTime? endDate = null, 
            Guid? serviceId = null, 
            Guid? clientId = null, 
            int skip = 0, 
            int take = 100);

        /// <summary>
        /// Met à jour les informations d'analyse IA d'une entrée de log.
        /// </summary>
        /// <param name="id">L'identifiant de l'entrée de log.</param>
        /// <param name="analysisResult">Le résultat de l'analyse IA.</param>
        /// <returns>True si mise à jour réussie, false sinon.</returns>
        Task<bool> UpdateAIAnalysisAsync(Guid id, string analysisResult);

        /// <summary>
        /// Obtient le nombre total de logs pour un service.
        /// </summary>
        /// <param name="serviceId">L'identifiant du service.</param>
        /// <returns>Le nombre total de logs pour le service.</returns>
        Task<int> GetCountByServiceIdAsync(Guid serviceId);

        /// <summary>
        /// Obtient le nombre total de logs pour un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <returns>Le nombre total de logs pour le client.</returns>
        Task<int> GetCountByClientIdAsync(Guid clientId);

        /// <summary>
        /// Obtient le nombre total de logs par niveau.
        /// </summary>
        /// <param name="level">Le niveau de log.</param>
        /// <returns>Le nombre total de logs pour le niveau spécifié.</returns>
        Task<int> GetCountByLevelAsync(LogLevel level);
    }
}