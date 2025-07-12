using AICO.Domain.DTOs.ValidationResults;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for statistical validation
/// Handles statistical significance and confidence level validation
/// </summary>
public interface IStatisticalValidationService
{
    /// <summary>
    /// Validate statistical significance requirements
    /// </summary>
    /// <param name="controlConversions">Control group conversions</param>
    /// <param name="variantConversions">Variant group conversions</param>
    /// <param name="minimumSampleSize">Minimum required sample size</param>
    /// <returns>Statistical validation result</returns>
    StatisticalValidationResult ValidateStatisticalSignificance(int controlConversions, int variantConversions, int minimumSampleSize = 30);

    /// <summary>
    /// Validate statistical significance requirements asynchronously
    /// </summary>
    /// <param name="controlConversions">Control group conversions</param>
    /// <param name="controlVisitors">Control group visitors</param>
    /// <param name="treatmentConversions">Treatment group conversions</param>
    /// <param name="treatmentVisitors">Treatment group visitors</param>
    /// <param name="confidenceLevel">Confidence level for validation</param>
    /// <returns>Statistical validation result</returns>
    Task<StatisticalValidationResult> ValidateStatisticalSignificanceAsync(int controlConversions, int controlVisitors, int treatmentConversions, int treatmentVisitors, double confidenceLevel = 0.95);

    /// <summary>
    /// Validate confidence level for statistical calculations
    /// </summary>
    /// <param name="confidenceLevel">Confidence level (0.0 to 1.0)</param>
    /// <returns>Confidence level validation result</returns>
    ConfidenceLevelValidationResult ValidateConfidenceLevel(double confidenceLevel);

    /// <summary>
    /// Validate confidence level for statistical calculations asynchronously
    /// </summary>
    /// <param name="confidenceLevel">Confidence level (0.0 to 1.0)</param>
    /// <returns>Confidence level validation result</returns>
    Task<ConfidenceLevelValidationResult> ValidateConfidenceLevelAsync(double confidenceLevel);
}