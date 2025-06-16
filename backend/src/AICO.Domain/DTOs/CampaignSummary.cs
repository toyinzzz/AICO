namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Summary information for a campaign
    /// </summary>
    public class CampaignSummary
    {
        /// <summary>
        /// Campaign ID
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Campaign name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Campaign status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Campaign start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Campaign end date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Total number of visitors
        /// </summary>
        public int TotalVisitors { get; set; }

        /// <summary>
        /// Total number of conversions
        /// </summary>
        public int TotalConversions { get; set; }

        /// <summary>
        /// Overall conversion rate
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// Best performing variant ID
        /// </summary>
        public Guid? BestVariantId { get; set; }

        /// <summary>
        /// Best performing variant name
        /// </summary>
        public string? BestVariantName { get; set; }

        /// <summary>
        /// Improvement percentage of best variant
        /// </summary>
        public decimal ImprovementPercentage { get; set; }

        /// <summary>
        /// Statistical significance of results
        /// </summary>
        public decimal StatisticalSignificance { get; set; }

        /// <summary>
        /// Whether results are statistically significant
        /// </summary>
        public bool IsSignificant { get; set; }

        /// <summary>
        /// Total revenue generated
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Revenue per visitor
        /// </summary>
        public decimal RevenuePerVisitor { get; set; }

        /// <summary>
        /// Estimated revenue lift
        /// </summary>
        public decimal RevenueLift { get; set; }

        /// <summary>
        /// Number of variants in the campaign
        /// </summary>
        public int VariantCount { get; set; }

        /// <summary>
        /// Campaign duration in days
        /// </summary>
        public int DurationDays { get; set; }

        /// <summary>
        /// Confidence level of the test
        /// </summary>
        public decimal ConfidenceLevel { get; set; }

        /// <summary>
        /// Test power
        /// </summary>
        public decimal TestPower { get; set; }

        /// <summary>
        /// Currency code for revenue metrics
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Summary generated timestamp
        /// </summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}