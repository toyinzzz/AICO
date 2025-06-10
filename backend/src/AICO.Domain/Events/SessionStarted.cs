using System;

namespace AICO.Domain.Events
{
    /// <summary>
    /// Domain event raised when a new session is started
    /// </summary>
    public class SessionStarted : DomainEvent
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
        /// The visitor ID
        /// </summary>
        public string VisitorId { get; }
        
        /// <summary>
        /// The entry page URL
        /// </summary>
        public string EntryPage { get; }
        
        public SessionStarted(Guid sessionId, Guid websiteId, string visitorId, string entryPage)
        {
            SessionId = sessionId;
            WebsiteId = websiteId;
            VisitorId = visitorId;
            EntryPage = entryPage;
        }
    }
}