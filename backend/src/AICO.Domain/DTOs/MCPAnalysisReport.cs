using AICO.Domain.ValueObjects;

namespace AICO.Domain.DTOs;

/// <summary>
/// Comprehensive MCP analysis report for MVP dashboard
/// </summary>
public class MCPAnalysisReport
{
    public Guid AbTestId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime ReportGeneratedAt { get; set; }
    public DateRange AnalysisPeriod { get; set; } = null!;
    
    // Overall MCP metrics
    public decimal OverallMCP { get; set; }
    public bool IsStatisticallySignificant { get; set; }
    public int TotalSampleSize { get; set; }
    public double ConfidenceLevel { get; set; }
    
    // Variant-specific MCP results
    public List<MCPResult> VariantMCPResults { get; set; } = new();
    
    // Revenue breakdown
    public decimal TotalRevenue { get; set; }
    public int TotalConversions { get; set; }
    public int TotalVisitors { get; set; }
    public decimal AverageOrderValue { get; set; }
    
    // Variant results
    public List<VariantComparison> VariantResults { get; set; } = new();
    
    // Control vs Best Variant comparison
    public VariantComparison? BestVariantComparison { get; set; }
    
    // Time-based analysis
    public List<MCPTrendPoint> MCPTrends { get; set; } = new();
    
    // Statistical insights
    public MCPStatisticalInsights StatisticalInsights { get; set; } = new();
    
    // Recommendations
    public List<string> Recommendations { get; set; } = new();
    
    // Currency information
    public string Currency { get; set; } = "USD";
    public bool IsCurrencyNormalized { get; set; }
}

/// <summary>
/// Comparison between control and best performing variant
/// </summary>
public class VariantComparison
{
    public Guid ControlVariantId { get; set; }
    public Guid BestVariantId { get; set; }
    public string VariantId { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public decimal MCP { get; set; }
    public decimal Revenue { get; set; }
    public int Conversions { get; set; }
    public decimal MCPImprovement { get; set; }
    public decimal RevenueIncrease { get; set; }
    public decimal ConversionRateIncrease { get; set; }
    public decimal ProjectedAnnualImpact { get; set; }
    public int Visitors { get; set; }
    public double ConversionRate { get; set; }
    public bool IsControl { get; set; }
}

/// <summary>
/// Statistical insights for MCP analysis
/// </summary>
public class MCPStatisticalInsights
{
    public double PValue { get; set; }
    public double ZScore { get; set; }
    public int SampleSize { get; set; }
    public int MinimumSampleSizeReached { get; set; }
    public int RecommendedSampleSize { get; set; }
    public double PowerAnalysis { get; set; }
    public double StatisticalPower { get; set; }
    public string SignificanceLevel { get; set; } = "95%";
    public bool HasSufficientData { get; set; }
    public double ConfidenceLevel { get; set; }
}