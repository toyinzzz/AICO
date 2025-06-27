using System;

namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Statistics for an entire A/B test
    /// </summary>
    public class AbTestStats
    {
        /// <summary>
        /// Total number of visitors across all variants
        /// </summary>
        public int TotalVisitors { get; set; }

        /// <summary>
        /// Total number of conversions across all variants
        /// </summary>
        public int TotalConversions { get; set; }

        /// <summary>
        /// When the test started
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Minimum detectable effect for the test
        /// </summary>
        public decimal MinimumDetectableEffect { get; set; }

        /// <summary>
        /// Statistical power of the test
        /// </summary>
        public decimal StatisticalPower { get; set; }
    }
}