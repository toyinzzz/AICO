using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for basic input validation
/// Handles fundamental data validation for MCP calculations
/// </summary>
public interface IBasicInputValidationService
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
}