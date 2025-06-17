using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.ValueObjects;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(AicoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetByWebsiteIdAsync(Guid websiteId)
        {
            return await _dbSet
                .Where(e => e.WebsiteId == websiteId)
                .Include(e => e.Website)
                .Include(e => e.Session)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetBySessionIdAsync(Guid sessionId)
        {
            return await _dbSet
                .Where(e => e.SessionId == sessionId)
                .Include(e => e.Website)
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByEventTypeAsync(EventType eventType)
        {
            return await _dbSet
                .Where(e => e.EventType == eventType)
                .Include(e => e.Website)
                .Include(e => e.Session)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate)
                .Include(e => e.Website)
                .Include(e => e.Session)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public Task<IEnumerable<Event>> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetByTypeAsync(Guid websiteId, string eventType)
        {
            throw new NotImplementedException();
        }

        public Task BulkInsertAsync(IEnumerable<Event> events)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountByTypeAsync(Guid websiteId, string eventType, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }
    }
}