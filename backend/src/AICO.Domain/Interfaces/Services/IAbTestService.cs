using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for A/B testing logic and variant management
    /// </summary>
    public interface IAbTestService
    {
        /// <summary>
        /// Creates a new A/B test with variants
        /// </summary>
        Task<AbTest> CreateTestAsync(Guid campaignId, List<Variant> variants, int trafficSplit = 50);
        
        /// <summary>
        /// Gets the appropriate variant for a visitor
        /// </summary>
        Task<Variant> GetVariantForVisitorAsync(Guid testId, string visitorId, string userAgent = null);
        
        /// <summary>
        /// Records a variant view/impression
        /// </summary>
        Task RecordVariantViewAsync(Guid variantId, string visitorId, string sessionId = null);
        
        /// <summary>
        /// Records a conversion for a variant
        /// </summary>
        Task RecordVariantConversionAsync(Guid variantId, string visitorId, decimal? value = null, string conversionType = null);
        
        /// <summary>
        /// Gets comprehensive test results
        /// </summary>
        Task<AbTestResults> GetTestResultsAsync(Guid testId);
        
        /// <summary>
        /// Checks if test has statistical significance
        /// </summary>
        Task<bool> IsTestStatisticallySignificantAsync(Guid testId, double confidenceLevel = 0.95);
        
        /// <summary>
        /// Calculates conversion rate for a variant
        /// </summary>
        Task<double> CalculateConversionRateAsync(Guid variantId);
        
        /// <summary>
        /// Declares a winning variant and stops the test
        /// </summary>
        Task<Variant> DeclareWinnerAsync(Guid testId, Guid? winningVariantId = null);
        
        /// <summary>
        /// Gets test performance over time
        /// </summary>
        Task<IEnumerable<TestPerformancePoint>> GetTestPerformanceHistoryAsync(Guid testId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Calculates required sample size for test
        /// </summary>
        Task<int> CalculateRequiredSampleSizeAsync(double baselineConversionRate, double minimumDetectableEffect, double power = 0.8, double significance = 0.05);
    }
}