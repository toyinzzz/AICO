using AICO.Domain.DTOs;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Interface for statistical analysis operations on A/B test results
    /// </summary>
    public interface IStatisticalAnalysisService
    {
        /// <summary>
        /// Calculates statistical significance between control and variant
        /// </summary>
        /// <param name="controlVisitors">Number of visitors in control group</param>
        /// <param name="controlConversions">Number of conversions in control group</param>
        /// <param name="variantVisitors">Number of visitors in variant group</param>
        /// <param name="variantConversions">Number of conversions in variant group</param>
        /// <param name="confidenceLevel">Confidence level (default 0.95 for 95%)</param>
        /// <returns>Statistical significance result</returns>
        StatisticalSignificanceResult CalculateSignificance(
            int controlVisitors, 
            int controlConversions, 
            int variantVisitors, 
            int variantConversions, 
            double confidenceLevel = 0.95);

        /// <summary>
        /// Calculates the minimum sample size needed for statistical significance
        /// </summary>
        /// <param name="baselineConversionRate">Expected baseline conversion rate</param>
        /// <param name="minimumDetectableEffect">Minimum effect size to detect</param>
        /// <param name="power">Statistical power (default 0.8)</param>
        /// <param name="alpha">Significance level (default 0.05)</param>
        /// <returns>Minimum sample size per variant</returns>
        int CalculateMinimumSampleSize(
            double baselineConversionRate, 
            double minimumDetectableEffect, 
            double power = 0.8, 
            double alpha = 0.05);

        /// <summary>
        /// Calculates confidence interval for conversion rate difference
        /// </summary>
        /// <param name="controlVisitors">Control group visitors</param>
        /// <param name="controlConversions">Control group conversions</param>
        /// <param name="variantVisitors">Variant group visitors</param>
        /// <param name="variantConversions">Variant group conversions</param>
        /// <param name="confidenceLevel">Confidence level</param>
        /// <returns>Confidence interval</returns>
        ConfidenceInterval CalculateConfidenceInterval(
            int controlVisitors, 
            int controlConversions, 
            int variantVisitors, 
            int variantConversions, 
            double confidenceLevel = 0.95);
    }
}