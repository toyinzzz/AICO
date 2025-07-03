using System;

namespace AICO.Domain.DTOs
{
    public class MCPStatisticalResult
    {
        public decimal MCP { get; set; }
        public bool IsStatisticallySignificant { get; set; }
        public double PValue { get; set; }
        public double ConfidenceLevel { get; set; }
        public decimal ControlRPC { get; set; }
        public decimal VariantRPC { get; set; }
        public double LiftPercentage { get; set; }
        public double ConfidenceIntervalLower { get; set; }
        public double ConfidenceIntervalUpper { get; set; }
        public int ControlSampleSize { get; set; }
        public int VariantSampleSize { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime CalculatedAt { get; set; }
        public string? Notes { get; set; }
        
        // Additional properties needed by MCPAnalyticsService
        public int Conversions { get; set; }
        public int Visitors { get; set; }
        public decimal RevenuePerVisitor { get; set; }
        public bool IsControl { get; set; }
        public decimal ControlMCP { get; set; }
        public decimal TreatmentMCP { get; set; }
        public double MCPImprovement { get; set; }
        public double ZScore { get; set; }
        public int SampleSize { get; set; }
    }
}