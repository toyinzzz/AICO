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
)
{
    /// <summary>
    /// Creates a default performance trends instance with safe default values
    /// </summary>
    public static MCPPerformanceTrends CreateDefault() => new(
        ConversionTrend: new List<MCPTrendDataPoint>(),
        RevenueTrend: new List<MCPTrendDataPoint>(),
        TrafficTrend: new List<MCPTrendDataPoint>(),
        TrendAnalysis: MCPTrendAnalysis.CreateDefault()
    );
}

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
)
{
    /// <summary>
    /// Creates a default trend analysis instance with safe default values
    /// </summary>
    public static MCPTrendAnalysis CreateDefault() => new(
        TrendDirection: string.Empty,
        TrendStrength: 0.0m,
        TrendFactors: new List<string>(),
        Seasonality: string.Empty
    );
}