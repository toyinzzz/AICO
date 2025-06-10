using System;
using System.Collections.Generic;
using AICO.Domain.Events;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a user session on a website
    /// </summary>
    public class Session : BaseEntity
    {
        /// <summary>
        /// The ID of the website this session belongs to
        /// </summary>
        public Guid WebsiteId { get; private set; }
        
        /// <summary>
        /// Unique identifier for the visitor (cookie-based)
        /// </summary>
        public string VisitorId { get; private set; }
        
        /// <summary>
        /// User agent string from the browser/client
        /// </summary>
        public string UserAgent { get; private set; }
        
        /// <summary>
        /// IP address of the client
        /// </summary>
        public string IpAddress { get; private set; }
        
        /// <summary>
        /// Referrer URL if available
        /// </summary>
        public string Referrer { get; private set; }
        
        /// <summary>
        /// First page visited in this session
        /// </summary>
        public string EntryPage { get; private set; }
        
        /// <summary>
        /// When the session started
        /// </summary>
        public DateTime StartedAt { get; private set; }
        
        /// <summary>
        /// When the session ended (null if still active)
        /// </summary>
        public DateTime? EndedAt { get; private set; }
        
        /// <summary>
        /// Duration of the session in seconds
        /// </summary>
        public int? DurationSeconds { get; private set; }
        
        /// <summary>
        /// Whether this session resulted in a conversion
        /// </summary>
        public bool HasConverted { get; private set; }
        
        /// <summary>
        /// Navigation property to the website
        /// </summary>
        public virtual Website Website { get; private set; }
        
        /// <summary>
        /// Collection of events in this session
        /// </summary>
        public virtual ICollection<Event> Events { get; private set; }
        
        // Private constructor for EF Core
        private Session()
        {
            Events = new List<Event>();
        }
        
        /// <summary>
        /// Creates a new session
        /// </summary>
        public static Session Create(
            Guid websiteId,
            string visitorId,
            string entryPage,
            string userAgent = null,
            string ipAddress = null,
            string referrer = null)
        {
            if (string.IsNullOrWhiteSpace(visitorId))
                throw new ArgumentException("Visitor ID cannot be null or empty", nameof(visitorId));
                
            if (string.IsNullOrWhiteSpace(entryPage))
                throw new ArgumentException("Entry page cannot be null or empty", nameof(entryPage));
                
            var session = new Session
            {
                WebsiteId = websiteId,
                VisitorId = visitorId,
                EntryPage = entryPage,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                Referrer = referrer,
                StartedAt = DateTime.UtcNow,
                HasConverted = false
            };
            
            session.AddDomainEvent(new SessionStarted(session.Id, websiteId, visitorId, entryPage));
            
            return session;
        }
        
        /// <summary>
        /// Ends the session
        /// </summary>
        public void End()
        {
            if (EndedAt.HasValue)
                return;
                
            EndedAt = DateTime.UtcNow;
            DurationSeconds = (int)(EndedAt.Value - StartedAt).TotalSeconds;
            MarkAsUpdated();
            
            AddDomainEvent(new SessionEnded(Id, WebsiteId, DurationSeconds.Value, HasConverted));
        }
        
        /// <summary>
        /// Marks the session as converted
        /// </summary>
        public void MarkAsConverted()
        {
            if (HasConverted)
                return;
                
            HasConverted = true;
            MarkAsUpdated();
        }
    }
}