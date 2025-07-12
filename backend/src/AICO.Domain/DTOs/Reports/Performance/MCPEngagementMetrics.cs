using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// User engagement metrics for performance analysis
/// </summary>
public record MCPEngagementMetrics(
    decimal AverageSessionDuration,
    decimal PagesPerSession,
    decimal BounceRate,
    decimal ReturnVisitorRate,
    Dictionary<string, decimal> InteractionRates,
    MCPUserBehaviorMetrics UserBehavior
)
{
    /// <summary>
    /// Creates a default engagement metrics instance with safe default values
    /// </summary>
    public static MCPEngagementMetrics CreateDefault() => new(
        AverageSessionDuration: 0.0m,
        PagesPerSession: 0.0m,
        BounceRate: 0.0m,
        ReturnVisitorRate: 0.0m,
        InteractionRates: new Dictionary<string, decimal>(),
        UserBehavior: MCPUserBehaviorMetrics.CreateDefault()
    );
}

/// <summary>
/// User behavior patterns and metrics
/// </summary>
public record MCPUserBehaviorMetrics(
    decimal ScrollDepth,
    decimal ClickHeatmapData,
    List<string> MostClickedElements,
    decimal FormCompletionRate,
    Dictionary<string, decimal> DevicePerformance
)
{
    /// <summary>
    /// Creates a default user behavior metrics instance with safe default values
    /// </summary>
    public static MCPUserBehaviorMetrics CreateDefault() => new(
        ScrollDepth: 0.0m,
        ClickHeatmapData: 0.0m,
        MostClickedElements: new List<string>(),
        FormCompletionRate: 0.0m,
        DevicePerformance: new Dictionary<string, decimal>()
    );
}