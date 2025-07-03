using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Comparison between test variants
/// </summary>
public record MCPVariantComparison(
    string VariantAId,
    string VariantBId,
    decimal ConversionRateDifference,
    decimal RevenueDifference,
    decimal StatisticalSignificance,
    string WinningVariant
);

/// <summary>
/// Statistical comparison analysis
/// </summary>
public record MCPStatisticalComparison(
    decimal PValue,
    decimal ConfidenceInterval,
    bool IsSignificant,
    string TestType,
    int SampleSize
);

/// <summary>
/// Performance gaps identified between variants
/// </summary>
public record MCPPerformanceGaps(
    List<string> IdentifiedGaps,
    Dictionary<string, decimal> GapSizes,
    List<string> ImprovementOpportunities
);

/// <summary>
/// Comparison of specific metrics between variants
/// </summary>
public record MCPMetricComparison(
    string MetricName,
    Dictionary<string, decimal> VariantValues,
    string BestPerformingVariant,
    decimal PerformanceGap
);

/// <summary>
/// Performance-based recommendations
/// </summary>
public record MCPRecommendations(
    List<string> ImmediateActions,
    List<string> LongTermStrategies,
    List<string> TestingRecommendations,
    string PriorityLevel
);