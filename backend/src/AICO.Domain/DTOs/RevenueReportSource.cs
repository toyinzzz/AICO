namespace AICO.Domain.DTOs
{
    public class RevenueReportSource
    {
        public Guid CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RevenueMetrics Metrics { get; set; }
        public decimal OverallMcp { get; set; }
        public string WinningVariant { get; set; }
    }
}