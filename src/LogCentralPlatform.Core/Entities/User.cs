using System;
using System.Collections.Generic;

namespace LogCentralPlatform.Core.Entities
{
    /// <summary>
    /// Représente un utilisateur du système.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom d'utilisateur.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Adresse e-mail de l'utilisateur.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Prénom de l'utilisateur.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Nom de famille de l'utilisateur.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Hash du mot de passe de l'utilisateur.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Sel de hachage du mot de passe.
        /// </summary>
        public string PasswordSalt { get; set; } = string.Empty;

        /// <summary>
        /// Date de création du compte.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date de dernière modification du compte.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Date de dernière connexion.
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Indique si le compte est actif.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indique si l'e-mail a été confirmé.
        /// </summary>
        public bool IsEmailConfirmed { get; set; } = false;

        /// <summary>
        /// Token de confirmation d'e-mail.
        /// </summary>
        public string? EmailConfirmationToken { get; set; }

        /// <summary>
        /// Date d'expiration du token de confirmation d'e-mail.
        /// </summary>
        public DateTime? EmailConfirmationTokenExpiresAt { get; set; }

        /// <summary>
        /// Token de réinitialisation de mot de passe.
        /// </summary>
        public string? PasswordResetToken { get; set; }

        /// <summary>
        /// Date d'expiration du token de réinitialisation de mot de passe.
        /// </summary>
        public DateTime? PasswordResetTokenExpiresAt { get; set; }

        /// <summary>
        /// Nombre de tentatives de connexion échouées consécutives.
        /// </summary>
        public int FailedLoginAttempts { get; set; } = 0;

        /// <summary>
        /// Date de verrouillage du compte.
        /// </summary>
        public DateTime? LockoutEndAt { get; set; }

        /// <summary>
        /// Indique si l'authentification à deux facteurs est activée.
        /// </summary>
        public bool TwoFactorEnabled { get; set; } = false;

        /// <summary>
        /// Secret pour l'authentification à deux facteurs.
        /// </summary>
        public string? TwoFactorSecret { get; set; }

        /// <summary>
        /// Rôles de l'utilisateur.
        /// </summary>
        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        /// <summary>
        /// Clients auxquels l'utilisateur a accès.
        /// </summary>
        public List<UserClientAccess> ClientAccess { get; set; } = new List<UserClientAccess>();

        /// <summary>
        /// Préférences de l'utilisateur.
        /// </summary>
        public UserPreferences Preferences { get; set; } = new UserPreferences();

        /// <summary>
        /// Numéro de téléphone de l'utilisateur.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Indique si le numéro de téléphone a été confirmé.
        /// </summary>
        public bool IsPhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// Obtient le nom complet de l'utilisateur.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    /// <summary>
    /// Rôle d'un utilisateur.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Identifiant unique du rôle d'utilisateur.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identifiant de l'utilisateur.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Nom du rôle.
        /// </summary>
        public string RoleName { get; set; } = string.Empty;

        /// <summary>
        /// Date d'attribution du rôle.
        /// </summary>
        public DateTime AssignedAt { get; set; }
    }

    /// <summary>
    /// Accès d'un utilisateur à un client.
    /// </summary>
    public class UserClientAccess
    {
        /// <summary>
        /// Identifiant unique de l'accès.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identifiant de l'utilisateur.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Identifiant du client.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Nom du client.
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Date d'attribution de l'accès.
        /// </summary>
        public DateTime AssignedAt { get; set; }

        /// <summary>
        /// Niveau d'accès (Lecture, Écriture, Admin, etc.).
        /// </summary>
        public AccessLevel AccessLevel { get; set; } = AccessLevel.Read;
    }

    /// <summary>
    /// Niveau d'accès à un client.
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// Accès en lecture seule.
        /// </summary>
        Read = 0,

        /// <summary>
        /// Accès en lecture et écriture.
        /// </summary>
        Write = 1,

        /// <summary>
        /// Accès administrateur (toutes les permissions).
        /// </summary>
        Admin = 2
    }

    /// <summary>
    /// Préférences d'un utilisateur.
    /// </summary>
    public class UserPreferences
    {
        /// <summary>
        /// Thème de l'interface (clair, sombre, etc.).
        /// </summary>
        public string Theme { get; set; } = "light";

        /// <summary>
        /// Langue de l'interface.
        /// </summary>
        public string Language { get; set; } = "fr";

        /// <summary>
        /// Fuseau horaire de l'utilisateur.
        /// </summary>
        public string TimeZone { get; set; } = "Europe/Paris";

        /// <summary>
        /// Format de date préféré.
        /// </summary>
        public string DateFormat { get; set; } = "dd/MM/yyyy";

        /// <summary>
        /// Format d'heure préféré.
        /// </summary>
        public string TimeFormat { get; set; } = "HH:mm";

        /// <summary>
        /// Indique si l'utilisateur souhaite recevoir des notifications par e-mail.
        /// </summary>
        public bool EmailNotificationsEnabled { get; set; } = true;

        /// <summary>
        /// Indique si l'utilisateur souhaite recevoir des notifications par SMS.
        /// </summary>
        public bool SmsNotificationsEnabled { get; set; } = false;

        /// <summary>
        /// Préférences supplémentaires au format JSON.
        /// </summary>
        public string? AdditionalPreferences { get; set; }
    }
}