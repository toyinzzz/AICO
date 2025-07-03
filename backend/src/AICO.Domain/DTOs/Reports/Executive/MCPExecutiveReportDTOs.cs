using System;
using System.Collections.Generic;
using AICO.Domain.DTOs.Common;

namespace AICO.Domain.DTOs.Reports.Executive;

/// <summary>
/// Executive summary report containing high-level business metrics and insights
/// </summary>
public class MCPExecutiveSummary
{
    public Guid AbTestId { get; set; }
    public string TestName { get; set; } = string.Empty;
    public DateTime ReportGeneratedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal OverallMCP { get; set; }
    public decimal MCPImprovement { get; set; }
    public string WinningVariant { get; set; } = string.Empty;
    public bool IsStatisticallySignificant { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalConversions { get; set; }
    public List<string> KeyInsights { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public MCPRiskAssessment RiskAssessment { get; set; } = new();
    
    // Keep nested objects for backward compatibility
    public MCPOverallMetrics OverallMetrics { get; set; } = new();
    public List<MCPVariantPerformance> VariantPerformance { get; set; } = new();
    public MCPStatisticalAnalysis StatisticalAnalysis { get; set; } = new();
    public MCPRevenueBreakdown RevenueBreakdown { get; set; } = new(0m, 0m, 0m, 0m, new Dictionary<string, decimal>(), new Dictionary<string, decimal>());
    public List<MCPPerformanceInsight> PerformanceInsights { get; set; } = new();
}



/// <summary>
/// Key performance insights for executive decision making
/// </summary>
public record MCPPerformanceInsight(
    string Category,
    string Insight,
    string Impact,
    decimal Confidence,
    List<string> SupportingData,
    string ActionRequired
);

/// <summary>
/// Risk assessment for executive awareness
/// </summary>
public class MCPRiskAssessment
{
    public string RiskLevel { get; set; } = string.Empty;
    public List<string> RiskFactors { get; set; } = new();
    public List<string> MitigationStrategies { get; set; } = new();
    public decimal RiskScore { get; set; }
    public string RecommendedAction { get; set; } = string.Empty;
    
    // Keep backward compatibility
    public List<string> IdentifiedRisks { get; set; } = new();
}