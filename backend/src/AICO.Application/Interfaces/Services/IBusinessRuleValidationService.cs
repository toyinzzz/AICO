using AICO.Application.DTOs;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.DTOs.Common;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for business rule validation
/// Handles industry-specific and business context validation
/// </summary>
public interface IBusinessRuleValidationService
{
    /// <summary>
    /// Validate business rules for MCP thresholds
    /// </summary>
    /// <param name="mcpValue">Calculated MCP value</param>
    /// <param name="businessContext">Business context for validation</param>
    /// <returns>Business rule validation result</returns>
    BusinessRuleValidationResult ValidateBusinessRules(decimal mcpValue, MCPBusinessContext businessContext);

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
    /// Validate A/B test configuration for MCP analysis
    /// </summary>
    /// <param name="abTestId">A/B test identifier</param>
    /// <param name="variants">Associated variants</param>
    /// <returns>A/B test validation result</returns>
    Task<AbTestValidationResult> ValidateAbTestConfigurationAsync(Guid abTestId, List<VariantData> variants);

    /// <summary>
    /// Comprehensive validation for MCP calculation request DTO
    /// </summary>
    Task<ComprehensiveValidationResult> ValidateComprehensiveMCPRequestAsync(MCPCalculationRequest request);
}