using AICO.Application.DTOs;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.DTOs.Common;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for MCP validation and business rule enforcement
/// Ensures data integrity and proper validation for MCP calculations
/// </summary>
public interface IMCPValidationService
{
    /// <summary>
    /// Validate basic MCP calculation inputs
    /// </summary>
    /// <param name="controlRevenue">Control group revenue</param>
    /// <param name="controlConversions">Control group conversions</param>
    /// <param name="variantRevenue">Variant group revenue</param>
    /// <param name="variantConversions">Variant group conversions</param>
    /// <returns>Validation result with detailed feedback</returns>
    MCPInputValidationResult ValidateBasicInputs(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions);

    /// <summary>
    /// Validate basic MCP calculation inputs asynchronously
    /// </summary>
    /// <param name="revenue">Revenue value</param>
    /// <param name="cost">Cost value</param>
    /// <param name="conversions">Number of conversions</param>
    /// <param name="visitors">Number of visitors</param>
    /// <returns>Validation result with detailed feedback</returns>
    Task<MCPInputValidationResult> ValidateBasicInputsAsync(decimal revenue, decimal cost, int conversions, int visitors);

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
    /// Validate currency inputs and normalization requirements
    /// </summary>
    /// <param name="currency">Currency code to validate</param>
    /// <returns>Currency validation result</returns>
    CurrencyValidationResult ValidateCurrency(string currency);

    /// <summary>
    /// Validate currency inputs and normalization requirements asynchronously
    /// </summary>
    /// <param name="amounts">List of amounts with currencies to validate</param>
    /// <param name="targetCurrency">Target currency for normalization</param>
    /// <returns>Currency validation result</returns>
    Task<CurrencyValidationResult> ValidateCurrencyAsync(List<(decimal amount, string currency)> amounts, string targetCurrency);

    /// <summary>
    /// Validate multiple variants data structure
    /// </summary>
    /// <param name="variants">List of variant data to validate</param>
    /// <returns>Multi-variant validation result</returns>
    MultiVariantValidationResult ValidateMultipleVariants(List<VariantData> variants);

    /// <summary>
    /// Validate multiple variants data structure asynchronously
    /// </summary>
    /// <param name="variants">List of variant data to validate</param>
    /// <returns>Multi-variant validation result</returns>
    Task<MultiVariantValidationResult> ValidateMultipleVariantsAsync(List<VariantData> variants);

    /// <summary>
    /// Validate date range for MCP analysis
    /// </summary>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <returns>Date range validation result</returns>
    DateRangeValidationResult ValidateDateRange(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Validate date range for MCP analysis asynchronously
    /// </summary>
    /// <param name="startDate">Analysis start date</param>
    /// <param name="endDate">Analysis end date</param>
    /// <returns>Date range validation result</returns>
    Task<DateRangeValidationResult> ValidateDateRangeAsync(DateTime startDate, DateTime endDate);

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

    /// <summary>
    /// Validate A/B test configuration for MCP analysis
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="variants">Associated variants</param>
    /// <returns>A/B test validation result</returns>
    Task<AbTestValidationResult> ValidateAbTestConfigurationAsync(Guid abTestId, List<VariantData> variants);

    /// <summary>
    /// Validate business rules for MCP thresholds
    /// </summary>
    /// <param name="mcpValue">Calculated MCP value</param>
    /// <param name="businessContext">Business context for validation</param>
    /// <returns>Business rule validation result</returns>
    BusinessRuleValidationResult ValidateBusinessRules(decimal mcpValue, MCPBusinessContext businessContext);

    /// <summary>
    /// Comprehensive validation for MCP calculation request DTO
    /// </summary>
    Task<ComprehensiveValidationResult> ValidateComprehensiveMCPRequestAsync(MCPCalculationRequest request);
}