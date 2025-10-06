namespace SpearFishure
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Static logging factory.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Gets or sets logger.
        /// </summary>
        public static ILoggerFactory? LoggerFactory { get; set; }
    }
}
