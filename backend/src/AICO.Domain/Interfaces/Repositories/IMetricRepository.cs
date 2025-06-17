using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository for Metric entity
    /// </summary>
    public interface IMetricRepository : IRepository<Metric>
    {
        /// <summary>
        /// Gets metrics by category for a website
        /// </summary>
        Task<IEnumerable<Metric>?> GetByCategoryAsync(Guid websiteId, string category, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets metrics by name for a website
        /// </summary>
        Task<IEnumerable<Metric>?> GetByNameAsync(Guid websiteId, string name, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets metrics by dimension for a website
        /// </summary>
        Task<IEnumerable<Metric>?> GetByDimensionAsync(Guid websiteId, string dimension, string? dimensionValue = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets the latest metric by name
        /// </summary>
        Task<Metric?> GetLatestAsync(Guid websiteId, string name, string? dimension = null, string? dimensionValue = null);

        /// <summary>
        /// Gets metrics for a website within a date range
        /// </summary>
        Task<IEnumerable<Metric>?> GetByWebsiteIdAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets metric trends aggregated by time period
        /// </summary>
        Task<IEnumerable<(DateTime Date, double Value)>?> GetTrendAsync(Guid websiteId, string name, string timeframe, string? dimension = null, string? dimensionValue = null);

        /// <summary>
        /// Bulk inserts multiple metrics
        /// </summary>
        Task BulkInsertAsync(IEnumerable<Metric> metrics);
    }
}