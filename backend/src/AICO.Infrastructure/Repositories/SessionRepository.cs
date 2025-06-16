using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories;

public class SessionRepository : BaseRepository<Session>, ISessionRepository
{
    public SessionRepository(AicoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Session>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Sessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetByWebsiteIdAsync(Guid websiteId)
    {
        return await _context.Sessions
            .Where(s => s.WebsiteId == websiteId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Session?> GetActiveSessionAsync(Guid userId, Guid websiteId)
    {
        return await _context.Sessions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.WebsiteId == websiteId && s.EndTime == null);
    }

    public async Task<IEnumerable<Session>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Sessions
            .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public Task<Session> GetActiveSessionAsync(Guid websiteId, string visitorId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid websiteId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Session>> GetByVisitorIdAsync(Guid websiteId, string visitorId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Session>> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Session>> GetConvertedSessionsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        throw new NotImplementedException();
    }

    public Task<(int TotalSessions, int ActiveSessions, int ConvertedSessions, double AverageDurationSeconds)> GetStatisticsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        throw new NotImplementedException();
    }

    Task IRepository<Session>.UpdateAsync(Session entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}