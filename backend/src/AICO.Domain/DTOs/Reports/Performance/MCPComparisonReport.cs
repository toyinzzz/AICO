using System;
using System.Collections.Generic;
using AICO.Domain.DTOs.Common;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Comprehensive comparison report between test variants
/// </summary>
public class MCPComparisonReport
{
    public DateTime ReportGeneratedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<MCPTestComparison> TestComparisons { get; set; } = new();
    public MCPBenchmarkAnalysis BenchmarkAnalysis { get; set; } = new();
    public MCPPortfolioMetrics PortfolioMetrics { get; set; } = new();
    
    // Keep existing properties for backward compatibility
    public string TestId { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public List<MCPVariantComparison> VariantComparisons { get; set; } = new();
    public MCPStatisticalComparison StatisticalComparison { get; set; } = MCPStatisticalComparison.CreateDefault();
    public MCPPerformanceGaps PerformanceGaps { get; set; } = MCPPerformanceGaps.CreateDefault();
    public List<MCPMetricComparison> MetricComparisons { get; set; } = new();
    public MCPRecommendations Recommendations { get; set; } = MCPRecommendations.CreateDefault();
}