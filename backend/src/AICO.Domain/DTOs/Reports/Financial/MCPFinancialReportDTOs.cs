using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Financial;

/// <summary>
/// Comprehensive ROI analysis report for financial performance
/// </summary>
public record MCPROIReport(
    string TestId,
    DateTime ReportDate,
    MCPROICalculation ROICalculation,
    List<MCPROIByVariant> VariantROI,
    MCPCostAnalysis CostAnalysis,
    MCPRevenueAnalysis RevenueAnalysis,
    List<MCPROIProjection> ROIProjections,
    MCPFinancialRecommendations Recommendations
);

/// <summary>
/// Statistical significance report for financial decisions
/// </summary>
public record MCPStatisticalReport(
    string TestId,
    DateTime ReportDate,
    List<MCPStatisticalTest> StatisticalTests,
    MCPPowerAnalysis PowerAnalysis,
    MCPSampleSizeAnalysis SampleSizeAnalysis,
    MCPConfidenceIntervals ConfidenceIntervals,
    MCPStatisticalRecommendations StatisticalRecommendations
);

/// <summary>
/// ROI calculation with detailed breakdown
/// </summary>
public record MCPROICalculation(
    decimal TotalROI,
    decimal ROIPercentage,
    decimal TotalInvestment,
    decimal TotalReturn,
    decimal NetProfit,
    decimal PaybackPeriod,
    string ROICategory
);

/// <summary>
/// ROI analysis by individual variant
/// </summary>
public record MCPROIByVariant(
    string VariantId,
    string VariantName,
    decimal VariantROI,
    decimal VariantInvestment,
    decimal VariantReturn,
    decimal VariantProfit,
    decimal ROIRank,
    MCPCostBreakdown CostBreakdown
);

/// <summary>
/// Detailed cost breakdown for variant
/// </summary>
public record MCPCostBreakdown(
    decimal DevelopmentCost,
    decimal TestingCost,
    decimal InfrastructureCost,
    decimal MarketingCost,
    decimal OperationalCost,
    decimal TotalCost
);

/// <summary>
/// Comprehensive cost analysis
/// </summary>
public record MCPCostAnalysis(
    decimal TotalTestCost,
    MCPCostByCategory CostByCategory,
    MCPCostTrends CostTrends,
    List<MCPCostDriver> CostDrivers,
    MCPCostEfficiency CostEfficiency
);

/// <summary>
/// Cost breakdown by category
/// </summary>
public record MCPCostByCategory(
    decimal PersonnelCosts,
    decimal TechnologyCosts,
    decimal InfrastructureCosts,
    decimal MarketingCosts,
    decimal OperationalCosts,
    decimal MiscellaneousCosts
);

/// <summary>
/// Cost trends over time
/// </summary>
public record MCPCostTrends(
    List<MCPCostDataPoint> CostHistory,
    decimal CostGrowthRate,
    string CostTrend,
    List<string> CostFactors
);

/// <summary>
/// Individual cost data point
/// </summary>
public record MCPCostDataPoint(
    DateTime Date,
    decimal Cost,
    string CostCategory,
    string Description
);

/// <summary>
/// Factors driving costs
/// </summary>
public record MCPCostDriver(
    string DriverName,
    decimal Impact,
    string Category,
    decimal CostContribution,
    string Controllability
);

/// <summary>
/// Cost efficiency metrics
/// </summary>
public record MCPCostEfficiency(
    decimal CostPerConversion,
    decimal CostPerVisitor,
    decimal CostPerRevenue,
    decimal EfficiencyRating,
    List<string> EfficiencyOpportunities
);

/// <summary>
/// Revenue analysis and breakdown
/// </summary>
public record MCPRevenueAnalysis(
    decimal TotalRevenue,
    MCPRevenueByVariant RevenueByVariant,
    MCPRevenueTrends RevenueTrends,
    List<MCPRevenueDriver> RevenueDrivers,
    MCPRevenueQuality RevenueQuality
);

/// <summary>
/// Revenue breakdown by variant
/// </summary>
public record MCPRevenueByVariant(
    Dictionary<string, decimal> VariantRevenues,
    string HighestRevenueVariant,
    decimal RevenueVariance,
    Dictionary<string, decimal> RevenueShares
);

/// <summary>
/// Revenue trends and patterns
/// </summary>
public record MCPRevenueTrends(
    List<MCPRevenueDataPoint> RevenueHistory,
    decimal RevenueGrowthRate,
    string RevenueTrend,
    MCPSeasonalRevenue SeasonalPatterns
);

/// <summary>
/// Individual revenue data point
/// </summary>
public record MCPRevenueDataPoint(
    DateTime Date,
    decimal Revenue,
    string VariantId,
    Dictionary<string, object> Metadata
);

/// <summary>
/// Seasonal revenue patterns
/// </summary>
public record MCPSeasonalRevenue(
    Dictionary<string, decimal> MonthlyPatterns,
    Dictionary<string, decimal> WeeklyPatterns,
    Dictionary<string, decimal> DailyPatterns,
    decimal SeasonalityStrength
);

/// <summary>
/// Factors driving revenue
/// </summary>
public record MCPRevenueDriver(
    string DriverName,
    decimal Impact,
    string Category,
    decimal RevenueContribution,
    string Optimization
);

/// <summary>
/// Quality metrics for revenue
/// </summary>
public record MCPRevenueQuality(
    decimal RecurringRevenuePercentage,
    decimal CustomerLifetimeValue,
    decimal RevenueStability,
    decimal RevenueConcentration,
    string QualityRating
);

/// <summary>
/// ROI projections for future periods
/// </summary>
public record MCPROIProjection(
    DateTime ProjectionDate,
    decimal ProjectedROI,
    decimal ProjectedRevenue,
    decimal ProjectedCosts,
    decimal Confidence,
    List<MCPROITimePoint> TimePoints
);

/// <summary>
/// ROI at specific time points
/// </summary>
public record MCPROITimePoint(
    DateTime Date,
    decimal ROI,
    decimal CumulativeROI,
    decimal Revenue,
    decimal Costs
);

/// <summary>
/// Financial recommendations based on analysis
/// </summary>
public record MCPFinancialRecommendations(
    List<string> ROIOptimizations,
    List<string> CostReductions,
    List<string> RevenueEnhancements,
    string InvestmentStrategy,
    MCPBudgetRecommendations BudgetRecommendations
);

/// <summary>
/// Budget allocation recommendations
/// </summary>
public record MCPBudgetRecommendations(
    Dictionary<string, decimal> RecommendedAllocations,
    decimal OptimalBudget,
    List<string> BudgetPriorities,
    string AllocationStrategy
);

/// <summary>
/// Individual statistical test result
/// </summary>
public record MCPStatisticalTest(
    string TestName,
    string TestType,
    decimal PValue,
    decimal TestStatistic,
    bool IsSignificant,
    decimal EffectSize,
    string Interpretation
);

/// <summary>
/// Power analysis for statistical tests
/// </summary>
public record MCPPowerAnalysis(
    decimal StatisticalPower,
    decimal EffectSize,
    decimal AlphaLevel,
    int SampleSize,
    string PowerInterpretation,
    List<string> PowerRecommendations
);

/// <summary>
/// Sample size analysis and recommendations
/// </summary>
public record MCPSampleSizeAnalysis(
    int CurrentSampleSize,
    int RecommendedSampleSize,
    decimal DetectableEffectSize,
    decimal TimeToReachSample,
    string SampleAdequacy
);

/// <summary>
/// Confidence intervals for key metrics
/// </summary>
public record MCPConfidenceIntervals(
    Dictionary<string, MCPConfidenceInterval> Intervals,
    decimal ConfidenceLevel,
    string IntervalMethod
);

/// <summary>
/// Individual confidence interval
/// </summary>
public record MCPConfidenceInterval(
    decimal LowerBound,
    decimal UpperBound,
    decimal PointEstimate,
    decimal MarginOfError
);

/// <summary>
/// Statistical recommendations for decision making
/// </summary>
public record MCPStatisticalRecommendations(
    List<string> TestingRecommendations,
    List<string> SampleSizeRecommendations,
    List<string> SignificanceRecommendations,
    string DecisionRecommendation,
    string ConfidenceAssessment
);