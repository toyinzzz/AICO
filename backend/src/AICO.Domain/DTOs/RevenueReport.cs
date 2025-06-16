namespace AICO.Domain.DTOs
{
    public class RevenueReport
    {
        public Guid AbTestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalConversions { get; set; }
        public decimal AverageOrderValue { get; set; }
        public Dictionary<Guid, decimal> RevenueByVariant { get; set; } = new();
        
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
        /// The ID of the variant with the highest statistically significant MCP
        /// </summary>
        public Guid? WinningVariantId { get; set; }
    }
}