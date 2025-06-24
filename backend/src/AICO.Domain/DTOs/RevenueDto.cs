using System;

namespace AICO.Domain.DTOs
{
    public class RevenueDto
    {
        public Guid Id { get; set; }
        public Guid WebsiteId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ConversionId { get; set; }
        public Guid CampaignId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Source { get; set; }
        public string TransactionId { get; set; }
        public string Metadata { get; set; }
        public DateTime RevenueDate { get; set; }
        public Guid? SessionId { get; set; }
        public Guid VariantId { get; set; }
        public bool IsControl { get; set; }
    }
}