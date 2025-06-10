using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository for Event entity
    /// </summary>
    public interface IEventRepository : IRepository<Event>
    {
        /// <summary>
        /// Gets events for a specific website
        /// </summary>
        Task<IEnumerable<Event>> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets events for a specific session
        /// </summary>
        Task<IEnumerable<Event>> GetBySessionIdAsync(Guid sessionId);
        
        /// <summary>
        /// Gets events by type for a specific website
        /// </summary>
        Task<IEnumerable<Event>> GetByTypeAsync(Guid websiteId, string eventType);
        
        /// <summary>
        /// Bulk inserts multiple events
        /// </summary>
        Task BulkInsertAsync(IEnumerable<Event> events);
        
        /// <summary>
        /// Gets count of events by type for a website
        /// </summary>
        Task<int> GetCountByTypeAsync(Guid websiteId, string eventType, DateTime? startDate = null, DateTime? endDate = null);
    }
}