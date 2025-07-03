using System;
using System.Collections.Generic;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.Common;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Detailed performance report with comprehensive metrics and analysis
/// </summary>
public class MCPPerformanceReport
{
    public Guid AbTestId { get; set; }
    public DateTime ReportGeneratedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MCPOverallMetrics OverallMetrics { get; set; } = new();
    public List<MCPVariantPerformance> VariantPerformance { get; set; } = new();
    public MCPStatisticalAnalysis StatisticalAnalysis { get; set; } = new();
    public List<MCPTrendPoint> DailyTrends { get; set; } = new();
    public MCPRevenueBreakdown RevenueBreakdown { get; set; } = new(0m, 0m, 0m, 0m, new Dictionary<string, decimal>(), new Dictionary<string, decimal>());
    
    // Keep existing properties for backward compatibility
    public string TestId { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string TimeRange { get; set; } = string.Empty;
    public List<MCPVariantMetrics> VariantMetrics { get; set; } = new();
    public MCPConversionFunnel ConversionFunnel { get; set; } = new(new List<MCPFunnelStep>(), 0m, new List<MCPDropOffPoint>(), new Dictionary<string, decimal>());
    public MCPEngagementMetrics EngagementMetrics { get; set; } = new(0m, 0m, 0m, 0m, new Dictionary<string, decimal>(), new MCPUserBehaviorMetrics(0m, 0m, new List<string>(), 0m, new Dictionary<string, decimal>()));
    public MCPPerformanceTrends PerformanceTrends { get; set; } = new(new List<MCPTrendDataPoint>(), new List<MCPTrendDataPoint>(), new List<MCPTrendDataPoint>(), new MCPTrendAnalysis(string.Empty, 0m, new List<string>(), string.Empty));
    public List<MCPSegmentPerformance> SegmentPerformance { get; set; } = new();
    public MCPGoalTracking GoalTracking { get; set; } = new(new List<MCPGoalMetric>(), 0m, new List<string>(), new List<string>());
}