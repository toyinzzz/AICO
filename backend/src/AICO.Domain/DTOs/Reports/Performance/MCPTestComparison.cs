using System;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Test comparison data for comparison reports
/// </summary>
public class MCPTestComparison
{
    public Guid AbTestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public decimal MCP { get; set; }
    public decimal MCPImprovement { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal Revenue { get; set; }
    public int SampleSize { get; set; }
    public bool IsStatisticallySignificant { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string WinningVariant { get; set; } = string.Empty;
}