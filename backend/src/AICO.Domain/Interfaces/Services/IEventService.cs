using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for managing events
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Creates a new event
        /// </summary>
        Task<Event> CreateEventAsync(Guid websiteId, string eventType, string eventData, string userAgent = null, string ipAddress = null, Guid? sessionId = null);
        
        /// <summary>
        /// Gets events for a specific website
        /// </summary>
        Task<IEnumerable<Event>> GetWebsiteEventsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets events for a specific session
        /// </summary>
        Task<IEnumerable<Event>> GetSessionEventsAsync(Guid sessionId);
        
        /// <summary>
        /// Gets events by type for a specific website
        /// </summary>
        Task<IEnumerable<Event>> GetEventsByTypeAsync(Guid websiteId, string eventType);
        
        /// <summary>
        /// Gets event by id
        /// </summary>
        Task<Event> GetEventByIdAsync(Guid eventId);
    }
}