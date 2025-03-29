using System;
using System.Collections.Generic;

namespace LogCentralPlatform.Core.Entities
{
    /// <summary>
    /// Représente un client dans le système.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Identifiant unique du client.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du client.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Numéro client ou identifiant externe.
        /// </summary>
        public string ClientNumber { get; set; } = string.Empty;

        /// <summary>
        /// Description ou informations supplémentaires.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Adresse e-mail principale du client.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Numéro de téléphone principal.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Adresse du client.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Date de création du client dans le système.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date de dernière mise à jour des informations du client.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Indique si le client est actif.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Contacts techniques du client pour les alertes et notifications.
        /// </summary>
        public List<ContactPerson> Contacts { get; set; } = new List<ContactPerson>();

        /// <summary>
        /// Paramètres de notification spécifiques au client.
        /// </summary>
        public NotificationSettings NotificationSettings { get; set; } = new NotificationSettings();

        /// <summary>
        /// Métadonnées supplémentaires liées au client.
        /// </summary>
        public Dictionary<string, string>? Metadata { get; set; }
    }

    /// <summary>
    /// Représente un contact associé à un client.
    /// </summary>
    public class ContactPerson
    {
        /// <summary>
        /// Identifiant unique du contact.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du contact.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Fonction ou rôle du contact.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Adresse e-mail du contact.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Numéro de téléphone du contact.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Indique si ce contact doit recevoir les notifications d'alertes.
        /// </summary>
        public bool ReceiveAlerts { get; set; } = false;
    }

    /// <summary>
    /// Paramètres de notification pour un client.
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// Indique si les notifications par e-mail sont activées.
        /// </summary>
        public bool EmailNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Indique si les notifications par SMS sont activées.
        /// </summary>
        public bool SmsNotificationsEnabled { get; set; } = false;

        /// <summary>
        /// Indique si les notifications par webhook sont activées.
        /// </summary>
        public bool WebhookNotificationsEnabled { get; set; } = false;

        /// <summary>
        /// URL du webhook pour les notifications.
        /// </summary>
        public string? WebhookUrl { get; set; }

        /// <summary>
        /// Niveau minimum des logs pour déclencher une notification.
        /// </summary>
        public LogLevel NotificationThreshold { get; set; } = LogLevel.Error;
    }
}