using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Segmentation;

/// <summary>
/// Comprehensive user segmentation analysis report
/// </summary>
public record MCPSegmentationReport(
    string TestId,
    DateTime ReportDate,
    List<MCPUserSegment> UserSegments,
    MCPSegmentationStrategy SegmentationStrategy,
    MCPSegmentPerformanceComparison PerformanceComparison,
    List<MCPSegmentInsight> SegmentInsights,
    MCPSegmentationRecommendations Recommendations
);

/// <summary>
/// Individual user segment with detailed metrics
/// </summary>
public record MCPUserSegment(
    string SegmentId,
    string SegmentName,
    string SegmentDescription,
    MCPSegmentCriteria Criteria,
    int SegmentSize,
    decimal SegmentPercentage,
    MCPSegmentMetrics Metrics,
    MCPSegmentBehavior Behavior,
    MCPSegmentDemographics Demographics
);

/// <summary>
/// Criteria used to define a user segment
/// </summary>
public record MCPSegmentCriteria(
    List<MCPSegmentRule> Rules,
    string LogicalOperator,
    Dictionary<string, object> Parameters,
    string SegmentationType
);

/// <summary>
/// Individual rule for segment definition
/// </summary>
public record MCPSegmentRule(
    string FieldName,
    string Operator,
    object Value,
    string DataType
);

/// <summary>
/// Performance metrics for a user segment
/// </summary>
public record MCPSegmentMetrics(
    decimal ConversionRate,
    decimal Revenue,
    decimal AverageOrderValue,
    decimal LifetimeValue,
    decimal EngagementScore,
    decimal RetentionRate,
    Dictionary<string, decimal> CustomMetrics
);

/// <summary>
/// Behavioral patterns of a user segment
/// </summary>
public record MCPSegmentBehavior(
    decimal AverageSessionDuration,
    decimal PagesPerSession,
    decimal BounceRate,
    List<string> PreferredChannels,
    Dictionary<string, decimal> InteractionPatterns,
    MCPPurchaseBehavior PurchaseBehavior
);

/// <summary>
/// Purchase behavior patterns
/// </summary>
public record MCPPurchaseBehavior(
    decimal AveragePurchaseFrequency,
    List<string> PreferredCategories,
    decimal Pricesensitivity,
    string PurchaseTiming,
    Dictionary<string, decimal> PaymentMethodPreferences
);

/// <summary>
/// Demographic information for a segment
/// </summary>
public record MCPSegmentDemographics(
    Dictionary<string, decimal> AgeDistribution,
    Dictionary<string, decimal> GenderDistribution,
    Dictionary<string, decimal> LocationDistribution,
    Dictionary<string, decimal> DeviceDistribution,
    Dictionary<string, decimal> IncomeDistribution
);

/// <summary>
/// Strategy used for segmentation
/// </summary>
public record MCPSegmentationStrategy(
    string StrategyName,
    string StrategyType,
    List<string> SegmentationVariables,
    string Algorithm,
    Dictionary<string, object> AlgorithmParameters,
    decimal SegmentationQuality
);

/// <summary>
/// Performance comparison between segments
/// </summary>
public record MCPSegmentPerformanceComparison(
    List<MCPSegmentComparison> Comparisons,
    string BestPerformingSegment,
    string WorstPerformingSegment,
    MCPPerformanceGapAnalysis GapAnalysis,
    List<MCPCrossSegmentInsight> CrossSegmentInsights
);

/// <summary>
/// Comparison between two segments
/// </summary>
public record MCPSegmentComparison(
    string SegmentA,
    string SegmentB,
    Dictionary<string, decimal> MetricDifferences,
    decimal StatisticalSignificance,
    string SignificantDifferences
);

/// <summary>
/// Analysis of performance gaps between segments
/// </summary>
public record MCPPerformanceGapAnalysis(
    List<string> IdentifiedGaps,
    Dictionary<string, decimal> GapSizes,
    List<string> GapCauses,
    List<string> ImprovementOpportunities
);

/// <summary>
/// Insights derived from cross-segment analysis
/// </summary>
public record MCPCrossSegmentInsight(
    string InsightType,
    string Description,
    List<string> AffectedSegments,
    decimal Impact,
    string ActionableRecommendation
);

/// <summary>
/// Insights specific to individual segments
/// </summary>
public record MCPSegmentInsight(
    string SegmentId,
    string InsightCategory,
    string Insight,
    decimal Confidence,
    List<string> SupportingData,
    MCPInsightImpact Impact,
    List<string> RecommendedActions
);

/// <summary>
/// Impact assessment for segment insights
/// </summary>
public record MCPInsightImpact(
    string ImpactLevel,
    decimal PotentialRevenue,
    decimal PotentialConversions,
    string TimeToImpact,
    decimal ImplementationEffort
);

/// <summary>
/// Recommendations based on segmentation analysis
/// </summary>
public record MCPSegmentationRecommendations(
    List<MCPSegmentRecommendation> SegmentSpecificRecommendations,
    List<string> OverallRecommendations,
    MCPPersonalizationStrategy PersonalizationStrategy,
    MCPTargetingStrategy TargetingStrategy,
    string PrioritySegment
);

/// <summary>
/// Recommendation specific to a segment
/// </summary>
public record MCPSegmentRecommendation(
    string SegmentId,
    string RecommendationType,
    string Recommendation,
    string Rationale,
    decimal ExpectedImpact,
    string ImplementationComplexity,
    string Priority
);

/// <summary>
/// Personalization strategy based on segmentation
/// </summary>
public record MCPPersonalizationStrategy(
    List<MCPPersonalizationRule> PersonalizationRules,
    Dictionary<string, string> SegmentPersonalization,
    string PersonalizationApproach,
    List<string> PersonalizationChannels
);

/// <summary>
/// Individual personalization rule
/// </summary>
public record MCPPersonalizationRule(
    string RuleId,
    string SegmentId,
    string PersonalizationType,
    Dictionary<string, object> PersonalizationParameters,
    decimal ExpectedLift
);

/// <summary>
/// Targeting strategy for different segments
/// </summary>
public record MCPTargetingStrategy(
    List<MCPTargetingRule> TargetingRules,
    Dictionary<string, decimal> SegmentPriorities,
    string TargetingApproach,
    MCPBudgetAllocation BudgetAllocation
);

/// <summary>
/// Individual targeting rule
/// </summary>
public record MCPTargetingRule(
    string RuleId,
    string SegmentId,
    string TargetingChannel,
    Dictionary<string, object> TargetingParameters,
    decimal ExpectedROI
);

/// <summary>
/// Budget allocation across segments
/// </summary>
public record MCPBudgetAllocation(
    Dictionary<string, decimal> SegmentBudgets,
    string AllocationStrategy,
    decimal TotalBudget,
    Dictionary<string, decimal> ExpectedReturns
);