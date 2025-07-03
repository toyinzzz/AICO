using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Performance trends over time
/// </summary>
public record MCPPerformanceTrends(
    List<MCPTrendDataPoint> ConversionTrend,
    List<MCPTrendDataPoint> RevenueTrend,
    List<MCPTrendDataPoint> TrafficTrend,
    MCPTrendAnalysis TrendAnalysis
);

/// <summary>
/// Individual data point in performance trends
/// </summary>
public record MCPTrendDataPoint(
    DateTime Date,
    decimal Value,
    string MetricType,
    string VariantId
);

/// <summary>
/// Analysis of performance trends
/// </summary>
public record MCPTrendAnalysis(
    string TrendDirection,
    decimal TrendStrength,
    List<string> TrendFactors,
    string Seasonality
);