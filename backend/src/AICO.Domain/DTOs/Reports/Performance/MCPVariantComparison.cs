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
)
{
    /// <summary>
    /// Creates a default statistical comparison instance with safe default values
    /// </summary>
    public static MCPStatisticalComparison CreateDefault() => new(
        PValue: 0.0m,
        ConfidenceInterval: 0.0m,
        IsSignificant: false,
        TestType: string.Empty,
        SampleSize: 0
    );
}

/// <summary>
/// Performance gaps identified between variants
/// </summary>
public record MCPPerformanceGaps(
    List<string> IdentifiedGaps,
    Dictionary<string, decimal> GapSizes,
    List<string> ImprovementOpportunities
)
{
    /// <summary>
    /// Creates a default performance gaps instance with safe default values
    /// </summary>
    public static MCPPerformanceGaps CreateDefault() => new(
        IdentifiedGaps: new List<string>(),
        GapSizes: new Dictionary<string, decimal>(),
        ImprovementOpportunities: new List<string>()
    );
}

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
)
{
    /// <summary>
    /// Creates a default recommendations instance with safe default values
    /// </summary>
    public static MCPRecommendations CreateDefault() => new(
        ImmediateActions: new List<string>(),
        LongTermStrategies: new List<string>(),
        TestingRecommendations: new List<string>(),
        PriorityLevel: string.Empty
    );
}