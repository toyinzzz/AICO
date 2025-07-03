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
);

/// <summary>
/// User behavior patterns and metrics
/// </summary>
public record MCPUserBehaviorMetrics(
    decimal ScrollDepth,
    decimal ClickHeatmapData,
    List<string> MostClickedElements,
    decimal FormCompletionRate,
    Dictionary<string, decimal> DevicePerformance
);