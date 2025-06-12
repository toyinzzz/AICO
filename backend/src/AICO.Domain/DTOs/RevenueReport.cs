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
        public Dictionary<Guid, decimal> RevenueByVariant { get; set; }
    }
}