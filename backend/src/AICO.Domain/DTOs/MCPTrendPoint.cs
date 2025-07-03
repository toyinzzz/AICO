namespace AICO.Domain.DTOs;

/// <summary>
/// Represents a single point in MCP trend analysis over time
/// </summary>
public class MCPTrendPoint
{
    public DateTime Timestamp { get; set; }
    public DateTime Date { get; set; }
    public decimal MCP { get; set; }
    public decimal CumulativeMCP { get; set; }
    public int SampleSize { get; set; }
    public int Visitors { get; set; }
    public bool IsStatisticallySignificant { get; set; }
    public decimal Revenue { get; set; }
    public int Conversions { get; set; }
    public decimal ConfidenceIntervalLower { get; set; }
    public decimal ConfidenceIntervalUpper { get; set; }
    public string Period { get; set; } = string.Empty; // "daily", "weekly", "monthly"
}

/// <summary>
/// MCP result with confidence intervals
/// </summary>
public class MCPConfidenceInterval
{
    public decimal MCP { get; set; }
    public decimal LowerBound { get; set; }
    public decimal UpperBound { get; set; }
    public double ConfidenceLevel { get; set; }
    public bool IsStatisticallySignificant { get; set; }
    public double MarginOfError { get; set; }
    public int SampleSize { get; set; }
}