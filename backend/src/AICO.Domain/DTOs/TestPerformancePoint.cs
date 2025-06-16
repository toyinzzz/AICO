namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Represents a single point in A/B test performance tracking
    /// </summary>
    public class TestPerformancePoint
    {
        /// <summary>
        /// Date/time of this data point
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Number of visitors for control variant
        /// </summary>
        public int ControlVisitors { get; set; }

        /// <summary>
        /// Number of conversions for control variant
        /// </summary>
        public int ControlConversions { get; set; }

        /// <summary>
        /// Conversion rate for control variant
        /// </summary>
        public decimal ControlConversionRate { get; set; }

        /// <summary>
        /// Number of visitors for test variant
        /// </summary>
        public int TestVisitors { get; set; }

        /// <summary>
        /// Number of conversions for test variant
        /// </summary>
        public int TestConversions { get; set; }

        /// <summary>
        /// Conversion rate for test variant
        /// </summary>
        public decimal TestConversionRate { get; set; }

        /// <summary>
        /// Improvement percentage over control
        /// </summary>
        public decimal ImprovementPercentage { get; set; }

        /// <summary>
        /// Statistical significance level
        /// </summary>
        public decimal StatisticalSignificance { get; set; }

        /// <summary>
        /// Confidence interval lower bound
        /// </summary>
        public decimal ConfidenceIntervalLower { get; set; }

        /// <summary>
        /// Confidence interval upper bound
        /// </summary>
        public decimal ConfidenceIntervalUpper { get; set; }

        /// <summary>
        /// Whether the result is statistically significant
        /// </summary>
        public bool IsSignificant { get; set; }

        /// <summary>
        /// P-value of the test
        /// </summary>
        public decimal PValue { get; set; }

        /// <summary>
        /// Total sample size at this point
        /// </summary>
        public int TotalSampleSize { get; set; }

        /// <summary>
        /// Test power at this sample size
        /// </summary>
        public decimal TestPower { get; set; }

        /// <summary>
        /// Period type (hourly, daily, weekly)
        /// </summary>
        public string PeriodType { get; set; } = "daily";
    }
}