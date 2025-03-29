namespace LogCentralPlatform.Web.Models
{
    /// <summary>
    /// Modèle de vue pour les erreurs.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Identifiant de la requête.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indique s'il faut afficher l'identifiant de la requête.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}