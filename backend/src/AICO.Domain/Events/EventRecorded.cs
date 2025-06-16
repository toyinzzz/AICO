using AICO.Domain.ValueObjects;

namespace AICO.Domain.Events
{
    /// <summary>
    /// Domain event raised when a new event is recorded
    /// </summary>
    public class EventRecorded : DomainEvent
    {
        /// <summary>
        /// The ID of the event
        /// </summary>
        public Guid EventId { get; }

        /// <summary>
        /// The ID of the website
        /// </summary>
        public Guid WebsiteId { get; }

        /// <summary>
        /// The type of event
        /// </summary>
        public EventType EventType { get; }

        /// <summary>
        /// The ID of the session (if available)
        /// </summary>
        public Guid? SessionId { get; }

        public EventRecorded(Guid eventId, Guid websiteId, EventType eventType, Guid? sessionId = null)
        {
            EventId = eventId;
            WebsiteId = websiteId;
            EventType = eventType;
            SessionId = sessionId;
        }
    }
}