using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository for Session entity
    /// </summary>
    public interface ISessionRepository : IRepository<Session>
    {
        /// <summary>
        /// Gets active session for a visitor
        /// </summary>
        Task<Session> GetActiveSessionAsync(Guid websiteId, string visitorId);

        /// <summary>
        /// Gets all active sessions for a website
        /// </summary>
        Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid websiteId);

        /// <summary>
        /// Gets all sessions for a specific visitor
        /// </summary>
        Task<IEnumerable<Session>> GetByVisitorIdAsync(Guid websiteId, string visitorId);

        /// <summary>
        /// Gets sessions for a website within a date range
        /// </summary>
        Task<IEnumerable<Session>> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets converted sessions for a website
        /// </summary>
        Task<IEnumerable<Session>> GetConvertedSessionsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets session statistics for a website
        /// </summary>
        Task<(int TotalSessions, int ActiveSessions, int ConvertedSessions, double AverageDurationSeconds)>
            GetStatisticsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);
    }
}