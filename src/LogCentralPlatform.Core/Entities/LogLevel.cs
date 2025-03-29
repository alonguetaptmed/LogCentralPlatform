namespace LogCentralPlatform.Core.Entities
{
    /// <summary>
    /// Niveaux de gravité des logs.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Information de débogage détaillée.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Information de débogage.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Information générale.
        /// </summary>
        Information = 2,

        /// <summary>
        /// Avertissement non critique.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Erreur applicative.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Erreur critique nécessitant une attention immédiate.
        /// </summary>
        Critical = 5
    }
}