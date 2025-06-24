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
            .FirstOrDefaultAsync(s => s.UserId == userId && s.WebsiteId == websiteId && s.EndedAt == null);
    }

    public async Task<IEnumerable<Session>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Sessions
            .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Session> GetActiveSessionAsync(Guid websiteId, string visitorId)
    {
        return await _context.Sessions
            .FirstAsync(s => s.WebsiteId == websiteId && s.VisitorId == visitorId && s.EndedAt == null);
    }

    public async Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid websiteId)
    {
        return await _context.Sessions
            .Where(s => s.WebsiteId == websiteId && s.EndedAt == null)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetByVisitorIdAsync(Guid websiteId, string visitorId)
    {
        return await _context.Sessions
            .Where(s => s.WebsiteId == websiteId && s.VisitorId == visitorId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Sessions.Where(s => s.WebsiteId == websiteId);
        if (startDate.HasValue)
            query = query.Where(s => s.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.CreatedAt <= endDate.Value);
        return await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetConvertedSessionsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Sessions.Where(s => s.WebsiteId == websiteId && s.HasConverted);
        if (startDate.HasValue)
            query = query.Where(s => s.StartedAt >= startDate.Value);
        if (endDate.HasValue)
            // Assuming if endDate is provided, we are looking for sessions that ended by this date.
            // If a session hasn't ended (EndedAt is null), it might still be considered if it started before endDate.
            // Adjust logic as per specific requirements for open-ended converted sessions.
            query = query.Where(s => s.EndedAt.HasValue && s.EndedAt.Value <= endDate.Value);
        return await query.OrderByDescending(s => s.StartedAt).ToListAsync();
    }

    public async Task<int> GetConvertedSessionsCountAsync(Guid websiteId, DateTime startDate, DateTime endDate)
    {
        return await _context.Sessions
            .CountAsync(s => s.WebsiteId == websiteId && s.HasConverted && s.StartedAt >= startDate && s.EndedAt <= endDate);
    }

    public async Task<IEnumerable<Session>> GetSessionsByVisitorAsync(string visitorId)
    {
        return await _context.Sessions
            .Where(s => s.VisitorId == visitorId)
            .OrderByDescending(s => s.StartedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid websiteId, int minutesAgo = 15)
    {
        var activeSince = DateTime.UtcNow.AddMinutes(-minutesAgo);
        return await _context.Sessions
            .Where(s => s.WebsiteId == websiteId && s.EndedAt == null && s.StartedAt >= activeSince)
            .OrderByDescending(s => s.StartedAt)
            .ToListAsync();
    }

    public async Task MarkSessionAsConvertedAsync(Guid sessionId)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            session.MarkAsConverted(); // Assuming Session entity has this method
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(int TotalSessions, int ActiveSessions, int ConvertedSessions, double AverageDurationSeconds)> GetStatisticsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        // Placeholder: Actual statistics calculation would be more involved.
        // This is a simplified example.
        // Returning default values for now.
        var query = _context.Sessions.Where(s => s.WebsiteId == websiteId);
        if (startDate.HasValue)
            query = query.Where(s => s.CreatedAt >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(s => s.CreatedAt <= endDate.Value);

        int totalSessions = await query.CountAsync();
        int activeSessions = await query.CountAsync(s => s.EndedAt == null);
        int convertedSessions = await query.CountAsync(s => s.HasConverted);
        // Average duration calculation would require EndedAt to be populated for completed sessions.
        // For simplicity, returning 0 if no completed sessions or if EndedAt is null.
        var completedSessions = await query.Where(s => s.EndedAt != null).ToListAsync();
        double averageDurationSeconds = completedSessions.Any() 
            ? completedSessions.Average(s => (s.EndedAt!.Value - s.CreatedAt).TotalSeconds) 
            : 0;

        return (totalSessions, activeSessions, convertedSessions, averageDurationSeconds);
    }
}