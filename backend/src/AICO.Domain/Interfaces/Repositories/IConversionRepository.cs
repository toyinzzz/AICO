using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository for Conversion entity
    /// </summary>
    public interface IConversionRepository : IRepository<Conversion>
    {
        /// <summary>
        /// Gets conversions for a website
        /// </summary>
        Task<IEnumerable<Conversion>> GetConversionsByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets conversions for a session
        /// </summary>
        Task<IEnumerable<Conversion>> GetConversionsBySessionIdAsync(Guid sessionId);

        /// <summary>
        /// Gets conversions by type
        /// </summary>
        Task<IEnumerable<Conversion>> GetConversionsByTypeAsync(Guid websiteId, string conversionType, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets conversions by goal
        /// </summary>
        Task<IEnumerable<Conversion>> GetConversionsByGoalAsync(Guid websiteId, string goalName, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets conversion trends grouped by timeframe
        /// </summary>
        /// <param name="websiteId">The website ID</param>
        /// <param name="timeframe">The timeframe to group by ("day" or "week")</param>
        /// <param name="goalName">Optional goal name to filter by</param>
        /// <param name="startDate">Optional start date</param>
        /// <param name="endDate">Optional end date</param>
        /// <returns>Collection of conversion trends with date, count and total value</returns>
        Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>> GetConversionTrendsAsync(Guid websiteId, string timeframe, string? goalName = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets conversion count for a website
        /// </summary>
        Task<int> GetCountAsync(Guid websiteId, string? goalName = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets total conversion value for a website
        /// </summary>
        Task<(decimal TotalValue, string? Currency)> GetTotalValueAsync(Guid websiteId, string? conversionType = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets conversion trends over time
        /// </summary>
        Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>?> GetTrendsAsync(Guid websiteId, string timeframe, string? goalName = null);

        /// <summary>
        /// Bulk inserts multiple conversions
        /// </summary>
        Task BulkInsertAsync(IEnumerable<Conversion> conversions);
    }
}