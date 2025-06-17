using AICO.Domain.Events;
using AICO.Domain.ValueObjects;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a user interaction event on a website
    /// </summary>
    public class Event : BaseEntity
    {
        /// <summary>
        /// The ID of the website where the event occurred
        /// </summary>
        public Guid WebsiteId { get; private set; }

        /// <summary>
        /// The ID of the session this event belongs to (optional)
        /// </summary>
        public Guid? SessionId { get; private set; }

        /// <summary>
        /// The type of event
        /// </summary>
        public EventType EventType { get; private set; }

        /// <summary>
        /// JSON data associated with the event
        /// </summary>
        public string? EventData { get; private set; }

        /// <summary>
        /// User agent string from the browser/client
        /// </summary>
        public string? UserAgent { get; private set; }

        /// <summary>
        /// IP address of the client
        /// </summary>
        public string? IpAddress { get; private set; }

        /// <summary>
        /// When the event occurred
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Navigation property to the website
        /// </summary>
        public virtual Website? Website { get; private set; }

        /// <summary>
        /// Navigation property to the session
        /// </summary>
        public virtual Session? Session { get; private set; }

        // Private constructor for EF Core
        private Event() { }

        /// <summary>
        /// Creates a new event
        /// </summary>
        public static Event Create(
            Guid websiteId,
            EventType eventType,
            string? eventData,
            string? userAgent = null,
            string? ipAddress = null,
            Guid? sessionId = null)
        {
            if (eventType == null)
                throw new ArgumentNullException(nameof(eventType));

            // Allow null or empty eventData if it's made nullable
            // if (string.IsNullOrWhiteSpace(eventData))
            //     throw new ArgumentException("Event data cannot be null or empty", nameof(eventData));

            var @event = new Event
            {
                WebsiteId = websiteId,
                SessionId = sessionId,
                EventType = eventType,
                EventData = eventData,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow
            };

            @event.AddDomainEvent(new EventRecorded(@event.Id, websiteId, eventType, sessionId));

            return @event;
        }

        /// <summary>
        /// Creates a new event
        /// </summary>
        public static Event Create(
            Guid websiteId,
            string eventType,
            string? eventData,
            string? userAgent = null,
            string? ipAddress = null,
            Guid? sessionId = null)
        {
            return Create(
                websiteId,
                EventType.Create(eventType),
                eventData,
                userAgent,
                ipAddress,
                sessionId);
        }
    }
}