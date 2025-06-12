using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for managing sessions
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Creates a new session or returns existing active session
        /// </summary>
        Task<Session> GetOrCreateSessionAsync(Guid websiteId, string visitorId, string entryPage, string userAgent = null, string ipAddress = null, string referrer = null);

        /// <summary>
        /// Gets a session by ID
        /// </summary>
        Task<Session> GetSessionByIdAsync(Guid sessionId);

        /// <summary>
        /// Gets active sessions for a website
        /// </summary>
        Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid websiteId);

        /// <summary>
        /// Gets sessions for a specific visitor
        /// </summary>
        Task<IEnumerable<Session>> GetVisitorSessionsAsync(Guid websiteId, string visitorId);

        /// <summary>
        /// Ends a session
        /// </summary>
        Task EndSessionAsync(Guid sessionId);

        /// <summary>
        /// Marks a session as converted
        /// </summary>
        Task MarkSessionAsConvertedAsync(Guid sessionId);

        /// <summary>
        /// Gets session statistics for a website
        /// </summary>
        Task<(int TotalSessions, int ActiveSessions, int ConvertedSessions, double AverageDurationSeconds)>
            GetSessionStatisticsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);
    }
}