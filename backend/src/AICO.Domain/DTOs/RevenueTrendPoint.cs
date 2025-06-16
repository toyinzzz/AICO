namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Represents a single point in revenue trend analysis
    /// </summary>
    public class RevenueTrendPoint
    {
        /// <summary>
        /// Date/time of this data point
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Revenue amount for this period
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// Number of transactions in this period
        /// </summary>
        public int TransactionCount { get; set; }

        /// <summary>
        /// Number of visitors in this period
        /// </summary>
        public int VisitorCount { get; set; }

        /// <summary>
        /// Conversion rate for this period
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// Average order value for this period
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// Revenue per visitor for this period
        /// </summary>
        public decimal RevenuePerVisitor { get; set; }

        /// <summary>
        /// Cumulative revenue up to this point
        /// </summary>
        public decimal CumulativeRevenue { get; set; }

        /// <summary>
        /// Revenue growth percentage compared to previous period
        /// </summary>
        public decimal? GrowthRate { get; set; }

        /// <summary>
        /// Variant ID if this is variant-specific data
        /// </summary>
        public Guid? VariantId { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Period type (hourly, daily, weekly, monthly)
        /// </summary>
        public string PeriodType { get; set; } = "daily";
    }
}