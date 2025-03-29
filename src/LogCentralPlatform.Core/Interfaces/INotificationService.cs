using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogCentralPlatform.Core.Entities;

namespace LogCentralPlatform.Core.Interfaces
{
    /// <summary>
    /// Interface pour le service de notification.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Envoie une notification d'erreur critique.
        /// </summary>
        /// <param name="logEntry">L'entrée de log concernée.</param>
        /// <param name="recipients">Les destinataires de la notification.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendCriticalErrorNotificationAsync(LogEntry logEntry, IEnumerable<string> recipients);
        
        /// <summary>
        /// Envoie une notification d'interruption de service.
        /// </summary>
        /// <param name="service">Le service concerné.</param>
        /// <param name="lastSeenAt">La dernière date d'activité du service.</param>
        /// <param name="recipients">Les destinataires de la notification.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendServiceInterruptionNotificationAsync(RegisteredService service, DateTime lastSeenAt, IEnumerable<string> recipients);
        
        /// <summary>
        /// Envoie une notification d'anomalie détectée par l'IA.
        /// </summary>
        /// <param name="anomaly">L'anomalie détectée.</param>
        /// <param name="service">Le service concerné.</param>
        /// <param name="recipients">Les destinataires de la notification.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendAIAnomalyNotificationAsync(AIAnomaly anomaly, RegisteredService service, IEnumerable<string> recipients);
        
        /// <summary>
        /// Envoie un rapport d'analyse périodique.
        /// </summary>
        /// <param name="report">Le rapport d'analyse.</param>
        /// <param name="recipients">Les destinataires du rapport.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendAnalysisReportAsync(AIAnalysisReport report, IEnumerable<string> recipients);
        
        /// <summary>
        /// Envoie une notification par SMS.
        /// </summary>
        /// <param name="message">Le message à envoyer.</param>
        /// <param name="phoneNumbers">Les numéros de téléphone destinataires.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendSmsNotificationAsync(string message, IEnumerable<string> phoneNumbers);
        
        /// <summary>
        /// Envoie une notification via webhook.
        /// </summary>
        /// <param name="payload">Les données à envoyer.</param>
        /// <param name="webhookUrl">L'URL du webhook.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendWebhookNotificationAsync(object payload, string webhookUrl);
        
        /// <summary>
        /// Envoie une notification générique par e-mail.
        /// </summary>
        /// <param name="subject">Le sujet de l'e-mail.</param>
        /// <param name="body">Le corps de l'e-mail.</param>
        /// <param name="recipients">Les destinataires de l'e-mail.</param>
        /// <param name="isHtml">Indique si le corps est au format HTML.</param>
        /// <returns>True si l'envoi a réussi, false sinon.</returns>
        Task<bool> SendEmailAsync(string subject, string body, IEnumerable<string> recipients, bool isHtml = true);
        
        /// <summary>
        /// Obtient les destinataires des notifications pour un service.
        /// </summary>
        /// <param name="service">Le service concerné.</param>
        /// <returns>La liste des adresses e-mail des destinataires.</returns>
        Task<IEnumerable<string>> GetNotificationRecipientsForServiceAsync(RegisteredService service);
        
        /// <summary>
        /// Obtient les destinataires des notifications pour un client.
        /// </summary>
        /// <param name="clientId">L'identifiant du client.</param>
        /// <returns>La liste des adresses e-mail des destinataires.</returns>
        Task<IEnumerable<string>> GetNotificationRecipientsForClientAsync(Guid clientId);
    }
}