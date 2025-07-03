using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Analytics;

/// <summary>
/// Comprehensive trend analysis report for data-driven insights
/// </summary>
public record MCPTrendAnalysis(
    string TestId,
    DateTime ReportDate,
    string TimeRange,
    List<MCPTrendStatistics> TrendStatistics,
    MCPSeasonalityAnalysis SeasonalityAnalysis,
    List<MCPTrendPattern> IdentifiedPatterns,
    MCPTrendPrediction TrendPrediction,
    List<MCPAnomalyDetection> Anomalies
);

/// <summary>
/// Predictive forecasting report for future performance estimation
/// </summary>
public record MCPForecastReport(
    string TestId,
    DateTime ForecastDate,
    string ForecastHorizon,
    List<MCPForecastPoint> ForecastPoints,
    MCPForecastAccuracy AccuracyMetrics,
    MCPForecastConfidence ConfidenceIntervals,
    List<MCPScenarioForecast> ScenarioForecasts,
    MCPForecastAssumptions Assumptions
);

/// <summary>
/// Real-time analytics dashboard data
/// </summary>
public record MCPDashboardData(
    DateTime LastUpdated,
    MCPRealTimeMetrics RealTimeMetrics,
    List<MCPLiveVariantData> LiveVariantData,
    MCPCurrentTrends CurrentTrends,
    List<MCPActiveAlert> ActiveAlerts,
    MCPSystemHealth SystemHealth
);

/// <summary>
/// Statistical trend data for analytics
/// </summary>
public record MCPTrendStatistics(
    string MetricName,
    List<MCPDataPoint> DataPoints,
    decimal TrendSlope,
    decimal RSquared,
    string TrendDirection,
    decimal Volatility,
    MCPMovingAverages MovingAverages
);

/// <summary>
/// Individual data point in trend analysis
/// </summary>
public record MCPDataPoint(
    DateTime Timestamp,
    decimal Value,
    string Category,
    Dictionary<string, object> Metadata
);

/// <summary>
/// Moving averages for trend smoothing
/// </summary>
public record MCPMovingAverages(
    decimal SevenDay,
    decimal ThirtyDay,
    decimal NinetyDay,
    decimal ExponentialMovingAverage
);

/// <summary>
/// Seasonality patterns in the data
/// </summary>
public record MCPSeasonalityAnalysis(
    bool HasSeasonality,
    string SeasonalityType,
    List<MCPSeasonalPattern> SeasonalPatterns,
    decimal SeasonalityStrength,
    Dictionary<string, decimal> SeasonalFactors
);

/// <summary>
/// Individual seasonal pattern
/// </summary>
public record MCPSeasonalPattern(
    string PatternType,
    string Period,
    decimal Amplitude,
    DateTime PeakTime,
    decimal Confidence
);

/// <summary>
/// Identified trend patterns
/// </summary>
public record MCPTrendPattern(
    string PatternName,
    string PatternType,
    DateTime StartDate,
    DateTime EndDate,
    decimal Strength,
    string Description
);

/// <summary>
/// Trend prediction and forecasting
/// </summary>
public record MCPTrendPrediction(
    List<MCPPredictionPoint> Predictions,
    decimal ConfidenceLevel,
    string PredictionModel,
    Dictionary<string, decimal> ModelParameters
);

/// <summary>
/// Individual prediction point
/// </summary>
public record MCPPredictionPoint(
    DateTime Date,
    decimal PredictedValue,
    decimal LowerBound,
    decimal UpperBound,
    decimal Confidence
);

/// <summary>
/// Anomaly detection results
/// </summary>
public record MCPAnomalyDetection(
    DateTime DetectedAt,
    string AnomalyType,
    decimal Severity,
    string Description,
    decimal ExpectedValue,
    decimal ActualValue,
    List<string> PossibleCauses
);

/// <summary>
/// Individual forecast point with confidence intervals
/// </summary>
public record MCPForecastPoint(
    DateTime Date,
    decimal ForecastValue,
    decimal LowerBound,
    decimal UpperBound,
    string MetricType,
    decimal Confidence
);

/// <summary>
/// Forecast accuracy metrics
/// </summary>
public record MCPForecastAccuracy(
    decimal MeanAbsoluteError,
    decimal MeanSquaredError,
    decimal MeanAbsolutePercentageError,
    decimal RSquared,
    string AccuracyRating
);

/// <summary>
/// Confidence intervals for forecasts
/// </summary>
public record MCPForecastConfidence(
    decimal ConfidenceLevel,
    Dictionary<string, MCPConfidenceBand> ConfidenceBands,
    string ConfidenceMethod
);

/// <summary>
/// Confidence band for forecast intervals
/// </summary>
public record MCPConfidenceBand(
    decimal LowerBound,
    decimal UpperBound,
    decimal Width
);

/// <summary>
/// Scenario-based forecasting
/// </summary>
public record MCPScenarioForecast(
    string ScenarioName,
    string ScenarioDescription,
    List<MCPForecastPoint> ForecastPoints,
    decimal Probability,
    Dictionary<string, object> ScenarioParameters
);

/// <summary>
/// Assumptions used in forecasting
/// </summary>
public record MCPForecastAssumptions(
    List<string> KeyAssumptions,
    Dictionary<string, decimal> ParameterAssumptions,
    List<string> RiskFactors,
    string ModelType
);

/// <summary>
/// Real-time metrics for dashboard
/// </summary>
public record MCPRealTimeMetrics(
    int CurrentVisitors,
    int ConversionsToday,
    decimal RevenueToday,
    decimal CurrentConversionRate,
    int ActiveTests,
    DateTime LastUpdate
);

/// <summary>
/// Live variant performance data
/// </summary>
public record MCPLiveVariantData(
    string VariantId,
    string VariantName,
    int CurrentVisitors,
    int ConversionsToday,
    decimal ConversionRate,
    decimal Revenue,
    string Status
);

/// <summary>
/// Current trending patterns
/// </summary>
public record MCPCurrentTrends(
    List<string> TrendingUp,
    List<string> TrendingDown,
    List<string> StableTrends,
    string OverallTrendDirection
);

/// <summary>
/// Active system alerts
/// </summary>
public record MCPActiveAlert(
    string AlertId,
    string AlertType,
    string Severity,
    string Message,
    DateTime TriggeredAt,
    bool IsAcknowledged
);

/// <summary>
/// System health monitoring
/// </summary>
public record MCPSystemHealth(
    string OverallStatus,
    decimal SystemLoad,
    decimal ResponseTime,
    int ErrorRate,
    DateTime LastHealthCheck,
    List<string> SystemIssues
);