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
    public MCPRevenueBreakdown RevenueBreakdown { get; set; } = MCPRevenueBreakdown.CreateDefault();
    
    // Keep existing properties for backward compatibility
    public string TestId { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string TimeRange { get; set; } = string.Empty;
    public List<MCPVariantMetrics> VariantMetrics { get; set; } = new();
    public MCPConversionFunnel ConversionFunnel { get; set; } = MCPConversionFunnel.CreateDefault();
    public MCPEngagementMetrics EngagementMetrics { get; set; } = MCPEngagementMetrics.CreateDefault();
    public MCPPerformanceTrends PerformanceTrends { get; set; } = MCPPerformanceTrends.CreateDefault();
    public List<MCPSegmentPerformance> SegmentPerformance { get; set; } = new();
    public MCPGoalTracking GoalTracking { get; set; } = MCPGoalTracking.CreateDefault();
}