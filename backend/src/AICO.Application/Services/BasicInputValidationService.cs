using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.ValidationResults;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for basic input validation
/// Handles fundamental data validation for MCP calculations
/// </summary>
public class BasicInputValidationService : IBasicInputValidationService
{
    private readonly ILogger<BasicInputValidationService> _logger;

    public BasicInputValidationService(ILogger<BasicInputValidationService> logger)
    {
        _logger = logger;
    }

    public MCPInputValidationResult ValidateBasicInputs(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        try
        {
            _logger.LogInformation("Validating basic MCP inputs: controlRevenue={ControlRevenue}, controlConversions={ControlConversions}, variantRevenue={VariantRevenue}, variantConversions={VariantConversions}", 
                controlRevenue, controlConversions, variantRevenue, variantConversions);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation rules
            if (controlRevenue < 0)
                validationErrors.Add("Control Revenue cannot be negative");
            if (controlConversions < 0)
                validationErrors.Add("Control Conversions cannot be negative");
            if (variantRevenue < 0)
                validationErrors.Add("Variant Revenue cannot be negative");
            if (variantConversions < 0)
                validationErrors.Add("Variant Conversions cannot be negative");

            // Business logic warnings
            if (controlConversions == 0 && variantConversions == 0)
                validationMessages.Add("No conversions detected in either group - MCP will be 0");

            if (controlRevenue == 0 && controlConversions > 0)
                validationMessages.Add("Control conversions detected with zero control revenue.");
            if (variantRevenue == 0 && variantConversions > 0)
                validationMessages.Add("Variant conversions detected with zero variant revenue.");

            if (controlConversions == 0 && controlRevenue > 0)
                validationMessages.Add("Control revenue detected with zero control conversions.");
            if (variantConversions == 0 && variantRevenue > 0)
                validationMessages.Add("Variant revenue detected with zero variant conversions.");

            // Calculate metrics
            var calculatedMCP = 0m;
            if (controlConversions > 0 && variantConversions > 0)
            {
                calculatedMCP = ((variantRevenue / variantConversions) - (controlRevenue / controlConversions));
            }

            if (controlRevenue > 0 && variantRevenue > 0 && calculatedMCP < 0)
                validationMessages.Add("Calculated MCP is negative, indicating variant performs worse than control.");

            var conversionRate = (controlConversions + variantConversions) > 0 ? 
                (double)(controlConversions + variantConversions) / (double)(controlRevenue + variantRevenue > 0 ? controlRevenue + variantRevenue : 1) : 0;
            var revenuePerVisitor = (controlRevenue + variantRevenue) > 0 ? 
                (controlRevenue + variantRevenue) / (controlConversions + variantConversions > 0 ? controlConversions + variantConversions : 1) : 0;
            var costPerConversion = 0m; // No direct cost parameter in this signature

            var isValid = !validationErrors.Any();

            var result = new MCPInputValidationResult
            {
                IsValid = isValid,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                CalculatedMCP = calculatedMCP,
                ConversionRate = (decimal)conversionRate,
                RevenuePerVisitor = revenuePerVisitor,
                CostPerConversion = costPerConversion
            };

            _logger.LogInformation("Basic validation completed. IsValid: {IsValid}, Errors: {ErrorCount}, Messages: {MessageCount}", 
                isValid, validationErrors.Count, validationMessages.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during basic MCP input validation");
            throw;
        }
    }

    public async Task<MCPInputValidationResult> ValidateBasicInputsAsync(decimal revenue, decimal cost, int conversions, int visitors)
    {
        try
        {
            _logger.LogInformation("Validating basic MCP inputs asynchronously: revenue={Revenue}, cost={Cost}, conversions={Conversions}, visitors={Visitors}", 
                revenue, cost, conversions, visitors);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation rules
            if (revenue < 0)
                validationErrors.Add("Revenue cannot be negative");
            if (cost < 0)
                validationErrors.Add("Cost cannot be negative");
            if (conversions < 0)
                validationErrors.Add("Conversions cannot be negative");
            if (visitors < 0)
                validationErrors.Add("Visitors cannot be negative");
            if (conversions > visitors)
                validationErrors.Add("Conversions cannot exceed visitors");

            // Business logic warnings
            if (visitors < 100)
                validationMessages.Add("Low visitor count may affect statistical significance");
            if (cost > revenue)
                validationMessages.Add("Cost exceeds revenue - negative profit margin");
            if (conversions == 0)
                validationMessages.Add("No conversions detected - MCP will be 0");

            // Calculate metrics
            var calculatedMCP = conversions > 0 ? (revenue - cost) / conversions : 0;
            var conversionRate = visitors > 0 ? (decimal)conversions / visitors : 0;
            var revenuePerVisitor = visitors > 0 ? revenue / visitors : 0;
            var costPerConversion = conversions > 0 ? cost / conversions : 0;

            var isValid = !validationErrors.Any();

            var result = new MCPInputValidationResult
            {
                IsValid = isValid,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                CalculatedMCP = calculatedMCP,
                ConversionRate = conversionRate,
                RevenuePerVisitor = revenuePerVisitor,
                CostPerConversion = costPerConversion
            };

            _logger.LogInformation("Basic async validation completed. IsValid: {IsValid}, Errors: {ErrorCount}, Messages: {MessageCount}", 
                isValid, validationErrors.Count, validationMessages.Count);

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during basic MCP input validation (async)");
            throw;
        }
    }

    public MultiVariantValidationResult ValidateMultipleVariants(List<VariantData> variants)
    {
        try
        {
            _logger.LogInformation("Validating {VariantCount} variants", variants?.Count ?? 0);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();
            var variantErrors = new List<VariantValidationError>();

            if (variants == null || !variants.Any())
            {
                validationErrors.Add("At least one variant is required");
                return new MultiVariantValidationResult
                {
                    IsValid = false,
                    ValidationErrors = validationErrors,
                    ValidationMessages = validationMessages,
                    VariantErrors = variantErrors
                };
            }

            // Check for control variant
            var controlVariants = variants.Where(v => v.IsControl).ToList();
            if (!controlVariants.Any())
                validationErrors.Add("At least one control variant is required");
            else if (controlVariants.Count > 1)
                validationErrors.Add("Only one control variant is allowed");

            // Validate each variant
            for (int i = 0; i < variants.Count; i++)
            {
                var variant = variants[i];
                var basicErrors = new List<string>();
                
                if (variant.Revenue < 0) basicErrors.Add("Revenue cannot be negative");
                if (variant.Cost < 0) basicErrors.Add("Cost cannot be negative");
                if (variant.Conversions < 0) basicErrors.Add("Conversions cannot be negative");
                if (variant.Visitors < 0) basicErrors.Add("Visitors cannot be negative");
                if (variant.Conversions > variant.Visitors) basicErrors.Add("Conversions cannot exceed visitors");
                
                if (basicErrors.Any())
                {
                    variantErrors.Add(new VariantValidationError
                    {
                        VariantIndex = i,
                        VariantName = variant.Name ?? $"Variant {i + 1}",
                        Errors = basicErrors
                    });
                }
            }

            var isValid = !validationErrors.Any() && !variantErrors.Any();
            var totalVisitors = variants.Sum(v => v.Visitors);

            return new MultiVariantValidationResult
            {
                IsValid = isValid,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                VariantErrors = variantErrors,
                HasControlVariant = variants?.Any(v => v.IsControl) ?? false,
                TotalSampleSize = totalVisitors,
                EstimatedTestDuration = TimeSpan.FromDays(CalculateEstimatedTestDuration(totalVisitors, variants?.Count ?? 0))
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during multi-variant validation");
            throw;
        }
    }

    public async Task<MultiVariantValidationResult> ValidateMultipleVariantsAsync(List<VariantData> variants)
    {
        return await Task.FromResult(ValidateMultipleVariants(variants));
    }

    private static int CalculateEstimatedTestDuration(int totalSampleSize, int variantCount)
    {
        // Simplified estimation: assume 1000 visitors per day
        var dailyTraffic = 1000;
        return Math.Max(7, totalSampleSize / dailyTraffic); // Minimum 7 days
    }
}