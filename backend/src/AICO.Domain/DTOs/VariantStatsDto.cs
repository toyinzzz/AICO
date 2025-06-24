namespace AICO.Domain.DTOs;
public class VariantStatsDto
    {
        public Guid VariantId { get; set; }
        public string? VariantName { get; set; }
        public int ConversionCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int Impressions { get; set; }
        public double ConversionRate { get; set; }
    }