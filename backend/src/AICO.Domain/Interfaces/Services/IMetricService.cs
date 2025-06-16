using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for managing metrics
    /// </summary>
    public interface IMetricService
    {
        /// <summary>
        /// Records a new metric
        /// </summary>
        Task<Metric> RecordMetricAsync(Guid websiteId, string name, string category, double value, string unit = null, string dimension = null, string dimensionValue = null);

        /// <summary>
        /// Updates an existing metric
        /// </summary>
        Task<Metric> UpdateMetricAsync(Guid metricId, double newValue);

        /// <summary>
        /// Gets metrics by category for a website
        /// </summary>
        Task<IEnumerable<Metric>> GetMetricsByCategoryAsync(Guid websiteId, string category, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets metrics by name for a website
        /// </summary>
        Task<IEnumerable<Metric>> GetMetricsByNameAsync(Guid websiteId, string name, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets metrics by dimension for a website
        /// </summary>
        Task<IEnumerable<Metric>> GetMetricsByDimensionAsync(Guid websiteId, string dimension, string dimensionValue = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets the latest value for a specific metric
        /// </summary>
        Task<Metric> GetLatestMetricAsync(Guid websiteId, string name, string dimension = null, string dimensionValue = null);

        /// <summary>
        /// Gets metric trends over time
        /// </summary>
        Task<IEnumerable<(DateTime Date, double Value)>> GetMetricTrendAsync(Guid websiteId, string name, string timeframe, string dimension = null, string dimensionValue = null);

        /// <summary>
        /// Calculates aggregate metrics for a dashboard
        /// </summary>
        Task<IDictionary<string, double>> CalculateDashboardMetricsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);
    }
}