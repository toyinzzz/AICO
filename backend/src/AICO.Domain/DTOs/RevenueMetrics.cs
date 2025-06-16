namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Comprehensive revenue metrics for a campaign
    /// </summary>
    public class RevenueMetrics
    {
        /// <summary>
        /// Campaign ID
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Total revenue generated
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Revenue per visitor
        /// </summary>
        public decimal RevenuePerVisitor { get; set; }

        /// <summary>
        /// Average order value
        /// </summary>
        public decimal AverageOrderValue { get; set; }

        /// <summary>
        /// Conversion rate
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// Total number of transactions
        /// </summary>
        public int TotalTransactions { get; set; }

        /// <summary>
        /// Total number of visitors
        /// </summary>
        public int TotalVisitors { get; set; }

        /// <summary>
        /// Revenue by variant
        /// </summary>
        public Dictionary<Guid, decimal> RevenueByVariant { get; set; } = new();

        /// <summary>
        /// Transactions by variant
        /// </summary>
        public Dictionary<Guid, int> TransactionsByVariant { get; set; } = new();

        /// <summary>
        /// Conversion rate by variant
        /// </summary>
        public Dictionary<Guid, decimal> ConversionRateByVariant { get; set; } = new();

        /// <summary>
        /// MCP (Marginal Contribution to Profit) by variant
        /// </summary>
        public Dictionary<Guid, decimal> MCPByVariant { get; set; } = new();

        /// <summary>
        /// Statistical significance of MCP values by variant
        /// </summary>
        public Dictionary<Guid, bool> MCPStatisticalSignificance { get; set; } = new();

        /// <summary>
        /// Overall MCP for the winning variant
        /// </summary>
        public decimal OverallMCP { get; set; }

        /// <summary>
        /// Revenue growth compared to control
        /// </summary>
        public decimal RevenueGrowth { get; set; }

        /// <summary>
        /// Statistical significance of results
        /// </summary>
        public decimal StatisticalSignificance { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// Period start date
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// Period end date
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// Metrics calculated timestamp
        /// </summary>
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}