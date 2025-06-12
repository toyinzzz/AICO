namespace AICO.Domain.Events
{
    /// <summary>
    /// Domain event raised when a session is ended
    /// </summary>
    public class SessionEnded : DomainEvent
    {
        /// <summary>
        /// The ID of the session
        /// </summary>
        public Guid SessionId { get; }

        /// <summary>
        /// The ID of the website
        /// </summary>
        public Guid WebsiteId { get; }

        /// <summary>
        /// The duration of the session in seconds
        /// </summary>
        public int DurationSeconds { get; }

        /// <summary>
        /// Whether the session resulted in a conversion
        /// </summary>
        public bool HasConverted { get; }

        public SessionEnded(Guid sessionId, Guid websiteId, int durationSeconds, bool hasConverted)
        {
            SessionId = sessionId;
            WebsiteId = websiteId;
            DurationSeconds = durationSeconds;
            HasConverted = hasConverted;
        }
    }
}