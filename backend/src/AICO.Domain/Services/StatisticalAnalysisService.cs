using AICO.Domain.DTOs;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for performing statistical analysis on A/B test results
    /// </summary>
    public class StatisticalAnalysisService : IStatisticalAnalysisService
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
        public StatisticalSignificanceResult CalculateSignificance(
            int controlVisitors, 
            int controlConversions, 
            int variantVisitors, 
            int variantConversions, 
            double confidenceLevel = 0.95)
        {
            if (controlVisitors <= 0 || variantVisitors <= 0)
            {
                return new StatisticalSignificanceResult
                {
                    IsSignificant = false,
                    PValue = 1.0,
                    ConfidenceLevel = confidenceLevel,
                    ZScore = 0,
                    ErrorMessage = "Insufficient sample size"
                };
            }

            var controlRate = (double)controlConversions / controlVisitors;
            var variantRate = (double)variantConversions / variantVisitors;

            // Calculate pooled standard error
            var pooledRate = (double)(controlConversions + variantConversions) / (controlVisitors + variantVisitors);
            var standardError = Math.Sqrt(pooledRate * (1 - pooledRate) * (1.0 / controlVisitors + 1.0 / variantVisitors));

            if (standardError == 0)
            {
                return new StatisticalSignificanceResult
                {
                    IsSignificant = false,
                    PValue = 1.0,
                    ConfidenceLevel = confidenceLevel,
                    ZScore = 0,
                    ErrorMessage = "Cannot calculate standard error"
                };
            }

            // Calculate Z-score
            var zScore = (variantRate - controlRate) / standardError;
            
            // Calculate p-value (two-tailed test)
            var pValue = 2 * (1 - NormalCDF(Math.Abs(zScore)));
            
            // Determine significance
            var alpha = 1 - confidenceLevel;
            var isSignificant = pValue < alpha;

            return new StatisticalSignificanceResult
            {
                IsSignificant = isSignificant,
                PValue = pValue,
                ConfidenceLevel = confidenceLevel,
                ZScore = zScore,
                ControlConversionRate = controlRate,
                VariantConversionRate = variantRate,
                Improvement = variantRate - controlRate,
                ImprovementPercentage = controlRate > 0 ? ((variantRate - controlRate) / controlRate) * 100 : 0
            };
        }

        /// <summary>
        /// Calculates the minimum sample size needed for statistical significance
        /// </summary>
        /// <param name="baselineConversionRate">Expected baseline conversion rate</param>
        /// <param name="minimumDetectableEffect">Minimum effect size to detect</param>
        /// <param name="power">Statistical power (default 0.8)</param>
        /// <param name="alpha">Significance level (default 0.05)</param>
        /// <returns>Minimum sample size per variant</returns>
        public int CalculateMinimumSampleSize(
            double baselineConversionRate, 
            double minimumDetectableEffect, 
            double power = 0.8, 
            double alpha = 0.05)
        {
            if (baselineConversionRate <= 0 || baselineConversionRate >= 1)
            {
                throw new ArgumentException("Baseline conversion rate must be between 0 and 1");
            }

            if (minimumDetectableEffect <= 0)
            {
                throw new ArgumentException("Minimum detectable effect must be greater than 0");
            }

            var p1 = baselineConversionRate;
            var p2 = baselineConversionRate + minimumDetectableEffect;
            
            if (p2 >= 1)
            {
                p2 = 0.99; // Cap at 99%
            }

            var pooledP = (p1 + p2) / 2;
            var zAlpha = InverseNormalCDF(1 - alpha / 2);
            var zBeta = InverseNormalCDF(power);

            var numerator = Math.Pow(zAlpha * Math.Sqrt(2 * pooledP * (1 - pooledP)) + zBeta * Math.Sqrt(p1 * (1 - p1) + p2 * (1 - p2)), 2);
            var denominator = Math.Pow(p2 - p1, 2);

            return (int)Math.Ceiling(numerator / denominator);
        }

        /// <summary>
        /// Calculates confidence interval for conversion rate difference
        /// </summary>
        /// <param name="controlVisitors">Control group visitors</param>
        /// <param name="controlConversions">Control group conversions</param>
        /// <param name="variantVisitors">Variant group visitors</param>
        /// <param name="variantConversions">Variant group conversions</param>
        /// <param name="confidenceLevel">Confidence level</param>
        /// <returns>Confidence interval</returns>
        public ConfidenceInterval CalculateConfidenceInterval(
            int controlVisitors, 
            int controlConversions, 
            int variantVisitors, 
            int variantConversions, 
            double confidenceLevel = 0.95)
        {
            var controlRate = (double)controlConversions / controlVisitors;
            var variantRate = (double)variantConversions / variantVisitors;
            var difference = variantRate - controlRate;

            var controlVariance = controlRate * (1 - controlRate) / controlVisitors;
            var variantVariance = variantRate * (1 - variantRate) / variantVisitors;
            var standardError = Math.Sqrt(controlVariance + variantVariance);

            var alpha = 1 - confidenceLevel;
            var zScore = InverseNormalCDF(1 - alpha / 2);
            var marginOfError = zScore * standardError;

            return new ConfidenceInterval
            {
                Lower = difference - marginOfError,
                Upper = difference + marginOfError,
                ConfidenceLevel = confidenceLevel
            };
        }

        /// <summary>
        /// Approximation of the cumulative distribution function for standard normal distribution
        /// </summary>
        private static double NormalCDF(double x)
        {
            return 0.5 * (1 + Erf(x / Math.Sqrt(2)));
        }

        /// <summary>
        /// Approximation of the inverse normal CDF
        /// </summary>
        private static double InverseNormalCDF(double p)
        {
            if (p <= 0 || p >= 1)
            {
                throw new ArgumentException("Probability must be between 0 and 1");
            }

            // Beasley-Springer-Moro algorithm approximation
            var c0 = 2.515517;
            var c1 = 0.802853;
            var c2 = 0.010328;
            var d1 = 1.432788;
            var d2 = 0.189269;
            var d3 = 0.001308;

            var t = Math.Sqrt(-2 * Math.Log(p < 0.5 ? p : 1 - p));
            var x = t - (c0 + c1 * t + c2 * t * t) / (1 + d1 * t + d2 * t * t + d3 * t * t * t);

            return p < 0.5 ? -x : x;
        }

        /// <summary>
        /// Error function approximation
        /// </summary>
        private static double Erf(double x)
        {
            var a1 = 0.254829592;
            var a2 = -0.284496736;
            var a3 = 1.421413741;
            var a4 = -1.453152027;
            var a5 = 1.061405429;
            var p = 0.3275911;

            var sign = x < 0 ? -1 : 1;
            x = Math.Abs(x);

            var t = 1.0 / (1.0 + p * x);
            var y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }
    }

}