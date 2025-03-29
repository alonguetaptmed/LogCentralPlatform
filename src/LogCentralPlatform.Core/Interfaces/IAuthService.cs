using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LogCentralPlatform.Core.Interfaces
{
    /// <summary>
    /// Interface pour le service d'authentification et d'autorisation.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authentifie un utilisateur avec son identifiant et mot de passe.
        /// </summary>
        /// <param name="username">Nom d'utilisateur ou email.</param>
        /// <param name="password">Mot de passe.</param>
        /// <returns>Le résultat de l'authentification.</returns>
        Task<AuthResult> AuthenticateAsync(string username, string password);
        
        /// <summary>
        /// Génère un token JWT pour un utilisateur authentifié.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="roles">Rôles de l'utilisateur.</param>
        /// <param name="claims">Claims additionnels.</param>
        /// <returns>Le token JWT généré.</returns>
        Task<string> GenerateJwtTokenAsync(Guid userId, IEnumerable<string> roles, IEnumerable<Claim>? additionalClaims = null);
        
        /// <summary>
        /// Vérifie la validité d'un token JWT.
        /// </summary>
        /// <param name="token">Le token à vérifier.</param>
        /// <returns>Le résultat de la validation.</returns>
        Task<TokenValidationResult> ValidateTokenAsync(string token);
        
        /// <summary>
        /// Vérifie l'authentification d'un service par clé API.
        /// </summary>
        /// <param name="apiKey">La clé API à vérifier.</param>
        /// <returns>Le résultat de l'authentification du service.</returns>
        Task<ServiceAuthResult> AuthenticateServiceAsync(string apiKey);
        
        /// <summary>
        /// Vérifie si un utilisateur a un rôle spécifique.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="role">Rôle à vérifier.</param>
        /// <returns>True si l'utilisateur a le rôle, false sinon.</returns>
        Task<bool> IsInRoleAsync(Guid userId, string role);
        
        /// <summary>
        /// Vérifie si un utilisateur a une permission spécifique.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="permission">Permission à vérifier.</param>
        /// <returns>True si l'utilisateur a la permission, false sinon.</returns>
        Task<bool> HasPermissionAsync(Guid userId, string permission);
        
        /// <summary>
        /// Vérifie si un utilisateur a accès à un client spécifique.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="clientId">Identifiant du client.</param>
        /// <returns>True si l'utilisateur a accès au client, false sinon.</returns>
        Task<bool> HasClientAccessAsync(Guid userId, Guid clientId);
        
        /// <summary>
        /// Vérifie si un utilisateur a accès à un service spécifique.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="serviceId">Identifiant du service.</param>
        /// <returns>True si l'utilisateur a accès au service, false sinon.</returns>
        Task<bool> HasServiceAccessAsync(Guid userId, Guid serviceId);
        
        /// <summary>
        /// Change le mot de passe d'un utilisateur.
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur.</param>
        /// <param name="currentPassword">Mot de passe actuel.</param>
        /// <param name="newPassword">Nouveau mot de passe.</param>
        /// <returns>True si le changement a réussi, false sinon.</returns>
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        
        /// <summary>
        /// Génère un token de réinitialisation de mot de passe.
        /// </summary>
        /// <param name="email">Email de l'utilisateur.</param>
        /// <returns>Le token généré ou null si l'email n'existe pas.</returns>
        Task<string?> GeneratePasswordResetTokenAsync(string email);
        
        /// <summary>
        /// Réinitialise le mot de passe avec un token.
        /// </summary>
        /// <param name="token">Token de réinitialisation.</param>
        /// <param name="newPassword">Nouveau mot de passe.</param>
        /// <returns>True si la réinitialisation a réussi, false sinon.</returns>
        Task<bool> ResetPasswordAsync(string token, string newPassword);
    }
    
    /// <summary>
    /// Résultat d'une authentification.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Indique si l'authentification a réussi.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Message d'erreur en cas d'échec.
        /// </summary>
        public string? ErrorMessage { get; set; }
        
        /// <summary>
        /// Identifiant de l'utilisateur authentifié.
        /// </summary>
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// Token JWT généré.
        /// </summary>
        public string? Token { get; set; }
        
        /// <summary>
        /// Date d'expiration du token.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
        
        /// <summary>
        /// Rôles de l'utilisateur.
        /// </summary>
        public List<string> Roles { get; set; } = new List<string>();
        
        /// <summary>
        /// Permissions de l'utilisateur.
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();
    }
    
    /// <summary>
    /// Résultat de la validation d'un token.
    /// </summary>
    public class TokenValidationResult
    {
        /// <summary>
        /// Indique si le token est valide.
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// Identifiant de l'utilisateur.
        /// </summary>
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// Claims extraits du token.
        /// </summary>
        public List<Claim> Claims { get; set; } = new List<Claim>();
        
        /// <summary>
        /// Message d'erreur en cas de token invalide.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
    
    /// <summary>
    /// Résultat de l'authentification d'un service.
    /// </summary>
    public class ServiceAuthResult
    {
        /// <summary>
        /// Indique si l'authentification a réussi.
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Identifiant du service authentifié.
        /// </summary>
        public Guid? ServiceId { get; set; }
        
        /// <summary>
        /// Nom du service authentifié.
        /// </summary>
        public string? ServiceName { get; set; }
        
        /// <summary>
        /// Identifiant du client associé au service.
        /// </summary>
        public Guid? ClientId { get; set; }
        
        /// <summary>
        /// Message d'erreur en cas d'échec.
        /// </summary>
        public string? ErrorMessage { get; set; }
        
        /// <summary>
        /// Environnement du service.
        /// </summary>
        public string? Environment { get; set; }
    }
}