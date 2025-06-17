using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for revenue tracking and profit calculations
    /// </summary>
    public interface IRevenueTrackingService
    {
        /// <summary>
        /// Records a revenue event for a specific variant
        /// </summary>
        Task RecordRevenueEventAsync(Guid campaignId, Guid variantId, decimal amount, string currency, string? transactionId = null);

        /// <summary>
        /// Calculates profit lift for a campaign
        /// </summary>
        Task<decimal> CalculateProfitLiftAsync(Guid campaignId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets comprehensive revenue metrics for a campaign
        /// </summary>
        Task<RevenueMetrics> GetRevenueMetricsAsync(Guid campaignId);

        /// <summary>
        /// Calculates ROI for a campaign
        /// </summary>
        Task<decimal> CalculateROIAsync(Guid campaignId);

        /// <summary>
        /// Gets revenue per visitor for each variant
        /// </summary>
        Task<Dictionary<Guid, decimal>> GetRevenuePerVisitorAsync(Guid campaignId);

        /// <summary>
        /// Calculates average order value by variant
        /// </summary>
        Task<Dictionary<Guid, decimal>> GetAverageOrderValueAsync(Guid campaignId);

        /// <summary>
        /// Gets revenue trends over time
        /// </summary>
        Task<IEnumerable<RevenueTrendPoint>> GetRevenueTrendsAsync(Guid campaignId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Calculates customer lifetime value impact
        /// </summary>
        Task<decimal> CalculateLifetimeValueImpactAsync(Guid campaignId);

        /// <summary>
        /// Gets profit attribution by traffic source
        /// </summary>
        Task<Dictionary<string, decimal>> GetProfitByTrafficSourceAsync(Guid campaignId);
    }
}