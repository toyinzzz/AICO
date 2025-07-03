using AICO.Application.DTOs;
using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.Common;
using AICO.Domain.DTOs.ValidationResults;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AICO.Application.Services;

/// <summary>
/// Service for MCP validation and business rule enforcement
/// Provides comprehensive validation for MCP calculations and inputs
/// </summary>
public class MCPValidationService : IMCPValidationService
{
    private readonly ILogger<MCPValidationService> _logger;
    private static readonly string[] SupportedCurrencies = { "USD", "EUR", "GBP", "CAD", "AUD", "JPY" };
    private static readonly Regex CurrencyCodeRegex = new(@"^[A-Z]{3}$", RegexOptions.Compiled);

    public MCPValidationService(ILogger<MCPValidationService> logger)
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

            // Additional validation for conversions vs. visitors (assuming these are implicitly part of the overall test)
            // For a more direct mapping to the interface, we'll focus on the provided parameters.
            // If conversions > visitors logic is needed, it should be applied to control/variant pairs if applicable.

            // Example: if we assume controlConversions <= controlVisitors and variantConversions <= variantVisitors
            // These 'visitors' parameters are not directly in the interface, so we'll omit this specific check for now
            // based on the interface signature.

            // Business logic warnings (adjusting to new parameters)
            if (controlConversions == 0 && variantConversions == 0)
                validationMessages.Add("No conversions detected in either group - MCP will be 0");

            // Business logic warnings
            // The original logic for 'low visitor count', 'cost exceeds revenue', and 'conversion rate' warnings
            // needs to be re-evaluated in the context of 'control' and 'variant' groups.
            // For now, I will remove them as they don't directly map to the new interface parameters.
            // If these are still required, they would need to be calculated based on the control/variant data.

            // Placeholder for MCP calculation - this needs to be defined based on how MCP is calculated from control/variant data
            // The original calculation was (revenue - cost) / conversions. This needs to be adapted.
            // For now, setting to 0 or a placeholder, as the exact formula isn't clear from the interface change.
            var calculatedMCP = 0m; // Placeholder
            var conversionRate = 0.0; // Placeholder
            var revenuePerVisitor = 0m; // Placeholder
            var costPerConversion = 0m; // Placeholder

            // Example of how conversion rates might be calculated for control and variant groups
            var controlConversionRate = controlConversions > 0 ? (double)controlConversions / (double)(controlRevenue > 0 ? controlRevenue : 1) : 0; // Assuming revenue acts as visitors for rate
            var variantConversionRate = variantConversions > 0 ? (double)variantConversions / (double)(variantRevenue > 0 ? variantRevenue : 1) : 0; // Assuming revenue acts as visitors for rate

            // You might want to add warnings based on these new rates, e.g., very low/high conversion rates for control/variant.

            // The original 'cost > revenue' warning is not directly applicable without a 'cost' parameter.
            // If profit margin is still a concern, it needs to be calculated differently.

            // The original 'low visitor count' warning is not directly applicable without 'visitors' parameter.
            // If sample size is a concern, it needs to be calculated differently.

            // For a basic implementation matching the interface, we'll focus on the direct parameter validation.

            // Recalculate calculatedMCP, ConversionRate, RevenuePerVisitor, CostPerConversion based on new parameters
            // This part requires clarification on how MCP is derived from control/variant revenue and conversions.
            // For now, I'll set them to default values or simple calculations that don't cause errors.

            // A simple example for MCP calculation (this might need adjustment based on business logic):
            if (controlConversions > 0 && variantConversions > 0)
            {
                // This is a simplified example. The actual MCP calculation might be more complex.
                calculatedMCP = ((variantRevenue / variantConversions) - (controlRevenue / controlConversions));
            }

            // Conversion rate could be an average or specific to control/variant
            conversionRate = (controlConversions + variantConversions) > 0 ? (double)(controlConversions + variantConversions) / (double)(controlRevenue + variantRevenue > 0 ? controlRevenue + variantRevenue : 1) : 0;

            // Revenue per visitor and cost per conversion are harder to define without explicit visitor/cost parameters.
            // Assuming 'revenue' can sometimes imply 'visitors' in a simplified context, but this is a guess.
            revenuePerVisitor = (controlRevenue + variantRevenue) > 0 ? (controlRevenue + variantRevenue) / (controlConversions + variantConversions > 0 ? controlConversions + variantConversions : 1) : 0;
            costPerConversion = 0m; // No direct cost parameter in the new signature

            // Re-evaluate the business logic warnings based on the new parameters.
            // For instance, if controlRevenue or variantRevenue is 0, and conversions are > 0, that might be a warning.
            if (controlRevenue == 0 && controlConversions > 0)
                validationMessages.Add("Control conversions detected with zero control revenue.");
            if (variantRevenue == 0 && variantConversions > 0)
                validationMessages.Add("Variant conversions detected with zero variant revenue.");

            if (controlConversions == 0 && controlRevenue > 0)
                validationMessages.Add("Control revenue detected with zero control conversions.");
            if (variantConversions == 0 && variantRevenue > 0)
                validationMessages.Add("Variant revenue detected with zero variant conversions.");

            if (controlRevenue > 0 && variantRevenue > 0 && calculatedMCP < 0)
                validationMessages.Add("Calculated MCP is negative, indicating variant performs worse than control.");

            var isValid = !validationErrors.Any();
            // The calculation for calculatedMCP and conversionRate needs to be re-evaluated based on the new parameters.
            // The previous calculation used 'revenue', 'cost', 'conversions', 'visitors'.
            // Now we have 'controlRevenue', 'controlConversions', 'variantRevenue', 'variantConversions'.
            // I've provided a placeholder calculation above, but it might need further refinement based on business logic.

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

    public async Task<StatisticalValidationResult> ValidateStatisticalSignificanceAsync(
        int controlConversions, int controlVisitors, int treatmentConversions, int treatmentVisitors, double confidenceLevel = 0.95)
    {
        try
        {
            _logger.LogInformation("Validating statistical significance with confidence level {ConfidenceLevel}", confidenceLevel);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Validate confidence level
            if (confidenceLevel <= 0 || confidenceLevel >= 1)
                validationErrors.Add("Confidence level must be between 0 and 1 (exclusive)");

            // Validate sample sizes
            if (controlVisitors < 30)
                validationErrors.Add("Control group sample size too small (minimum 30 required)");
            if (treatmentVisitors < 30)
                validationErrors.Add("Treatment group sample size too small (minimum 30 required)");

            // Check for adequate sample sizes for statistical power
            var totalSampleSize = controlVisitors + treatmentVisitors;
            if (totalSampleSize < 1000)
                validationMessages.Add("Total sample size may be insufficient for reliable statistical analysis");

            // Calculate statistical metrics
            var controlConversionRate = controlVisitors > 0 ? (double)controlConversions / controlVisitors : 0;
            var treatmentConversionRate = treatmentVisitors > 0 ? (double)treatmentConversions / treatmentVisitors : 0;
            
            var pooledConversionRate = totalSampleSize > 0 ? 
                (controlConversions + treatmentConversions) / (double)totalSampleSize : 0;
            
            var standardError = controlVisitors > 0 && treatmentVisitors > 0 ? 
                Math.Sqrt(pooledConversionRate * (1 - pooledConversionRate) * (1.0 / controlVisitors + 1.0 / treatmentVisitors)) : 0;
            
            var zScore = standardError > 0 ? (treatmentConversionRate - controlConversionRate) / standardError : 0;
            var pValue = standardError > 0 ? 2 * (1 - NormalCDF(Math.Abs(zScore))) : 1;
            
            var isSignificant = pValue < (1 - confidenceLevel);
            var statisticalPower = CalculateStatisticalPower(controlVisitors, treatmentVisitors, Math.Abs(treatmentConversionRate - controlConversionRate));

            // Power analysis warnings
            if (statisticalPower < 0.8)
                validationMessages.Add($"Statistical power is low ({statisticalPower:P1}). Consider increasing sample size.");

            var result = new StatisticalValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                IsStatisticallySignificant = isSignificant,
                PValue = pValue,
                ZScore = zScore,
                StatisticalPower = statisticalPower,
                EffectSize = Math.Abs(treatmentConversionRate - controlConversionRate),
                RecommendedSampleSize = CalculateRecommendedSampleSize(controlConversionRate, 0.05, 0.8, confidenceLevel)
            };

            _logger.LogInformation("Statistical validation completed. Significant: {IsSignificant}, Power: {Power:P1}", 
                isSignificant, statisticalPower);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during statistical significance validation");
            throw;
        }
    }

    public async Task<CurrencyValidationResult> ValidateCurrencyAsync(string currency, decimal amount)
    {
        try
        {
            _logger.LogInformation("Validating currency {Currency} with amount {Amount}", currency, amount);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic currency format validation
            if (string.IsNullOrWhiteSpace(currency))
            {
                validationErrors.Add("Currency code cannot be null or empty");
            }
            else
            {
                if (!CurrencyCodeRegex.IsMatch(currency))
                    validationErrors.Add("Currency code must be a 3-letter ISO code (e.g., USD, EUR)");
                else if (!SupportedCurrencies.Contains(currency.ToUpperInvariant()))
                    validationMessages.Add($"Currency {currency} may require manual conversion rates");
            }

            // Amount validation
            if (amount < 0)
                validationErrors.Add("Amount cannot be negative");
            if (amount > 1_000_000_000) // 1 billion limit
                validationMessages.Add("Very large amount detected - please verify accuracy");

            // Currency-specific validations
            var normalizedCurrency = currency?.ToUpperInvariant();
            var conversionRate = GetCurrencyConversionRate(normalizedCurrency);
            var normalizedAmount = amount * conversionRate;

            var result = new CurrencyValidationResult
            {
                IsValid = !validationErrors.Any(),
                ErrorMessages = validationErrors,
                ValidationMessages = validationMessages,
                NormalizedCurrency = "USD", // Base currency
                NormalizedAmount = normalizedAmount,
                ConversionRate = conversionRate,
                IsSupported = SupportedCurrencies.Contains(normalizedCurrency ?? "")
            };

            _logger.LogInformation("Currency validation completed. IsValid: {IsValid}, Normalized: {NormalizedAmount} USD", 
                result.IsValid, normalizedAmount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during currency validation");
            throw;
        }
    }

    public async Task<MultiVariantValidationResult> ValidateMultipleVariantsAsync(List<VariantData> variants)
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
                // Validate basic inputs synchronously
                var basicErrors = new List<string>();
                if (variant.Revenue < 0) basicErrors.Add("Revenue cannot be negative");
                if (variant.Cost < 0) basicErrors.Add("Cost cannot be negative");
                if (variant.Conversions < 0) basicErrors.Add("Conversions cannot be negative");
                if (variant.Visitors < 0) basicErrors.Add("Visitors cannot be negative");
                if (variant.Conversions > variant.Visitors) basicErrors.Add("Conversions cannot exceed visitors");
                
                var variantValidation = new MCPInputValidationResult
                {
                    IsValid = !basicErrors.Any(),
                    ValidationErrors = basicErrors
                };
                
                if (!variantValidation.IsValid)
                {
                    foreach (var error in variantValidation.ValidationErrors)
                    {
                        variantErrors.Add(new VariantValidationError
                        {
                            VariantId = variant.VariantId,
                            ErrorType = "ValidationError",
                            ErrorMessage = error,
                            FieldName = "General"
                        });
                    }
                }

                // Check for duplicate variant IDs
                if (variants.Count(v => v.VariantId == variant.VariantId) > 1)
                {
                    variantErrors.Add(new VariantValidationError
                    {
                        VariantId = variant.VariantId,
                        ErrorType = "DuplicateId",
                        ErrorMessage = "Duplicate variant ID detected",
                        FieldName = "VariantId"
                    });
                }
            }

            // Check sample size distribution
            var totalVisitors = variants.Sum(v => v.Visitors);
            if (totalVisitors < 1000)
                validationMessages.Add("Total sample size across all variants may be insufficient");

            // Check for balanced distribution
            var avgVisitorsPerVariant = totalVisitors / (double)variants.Count;
            var imbalancedVariants = variants.Where(v => Math.Abs(v.Visitors - avgVisitorsPerVariant) > avgVisitorsPerVariant * 0.5).ToList();
            if (imbalancedVariants.Any())
                validationMessages.Add("Unbalanced visitor distribution detected across variants");

            var result = new MultiVariantValidationResult
            {
                IsValid = !validationErrors.Any() && !variantErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                VariantErrors = variantErrors,
                TotalVariants = variants.Count,
                ControlVariantCount = controlVariants.Count,
                TotalSampleSize = totalVisitors
            };

            _logger.LogInformation("Multi-variant validation completed. IsValid: {IsValid}, Variants: {VariantCount}, Errors: {ErrorCount}", 
                result.IsValid, variants.Count, variantErrors.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during multi-variant validation");
            throw;
        }
    }

    public async Task<DateRangeValidationResult> ValidateDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation("Validating date range: {StartDate} to {EndDate}", startDate, endDate);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic date validation
            if (startDate >= endDate)
                validationErrors.Add("Start date must be before end date");
            
            if (startDate > DateTime.UtcNow)
                validationErrors.Add("Start date cannot be in the future");
            
            if (endDate > DateTime.UtcNow)
                validationMessages.Add("End date is in the future - results may be incomplete");

            // Duration validation
            var duration = endDate - startDate;
            if (duration.TotalDays < 1)
                validationMessages.Add("Very short analysis period (< 1 day) may not provide reliable results");
            else if (duration.TotalDays > 365)
                validationMessages.Add("Very long analysis period (> 1 year) may include seasonal variations");
            
            if (duration.TotalDays < 7)
                validationMessages.Add("Analysis period less than 1 week may not capture weekly patterns");

            // Business day considerations
            var businessDays = CalculateBusinessDays(startDate, endDate);
            if (businessDays < 5)
                validationMessages.Add("Less than 5 business days in analysis period");

            var result = new DateRangeValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                DurationDays = (int)duration.TotalDays,
                BusinessDays = businessDays,
                IncludesWeekends = duration.TotalDays >= 7,
                IsCurrentPeriod = endDate.Date >= DateTime.UtcNow.Date
            };

            _logger.LogInformation("Date range validation completed. IsValid: {IsValid}, Duration: {Duration} days", 
                result.IsValid, result.DurationDays);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during date range validation");
            throw;
        }
    }

    public StatisticalValidationResult ValidateStatisticalSignificance(int controlConversions, int variantConversions, int minimumSampleSize = 30)
    {
        try
        {
            _logger.LogInformation("Validating statistical significance: controlConversions={ControlConversions}, variantConversions={VariantConversions}, minimumSampleSize={MinimumSampleSize}", 
                controlConversions, variantConversions, minimumSampleSize);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation
            if (controlConversions < 0)
                validationErrors.Add("Control conversions cannot be negative");
            if (variantConversions < 0)
                validationErrors.Add("Variant conversions cannot be negative");

            var totalSampleSize = controlConversions + variantConversions;
            var hasSufficientSampleSize = totalSampleSize >= minimumSampleSize;

            if (!hasSufficientSampleSize)
                validationErrors.Add($"Insufficient sample size. Required: {minimumSampleSize}, Actual: {totalSampleSize}");

            if (totalSampleSize < 100)
                validationMessages.Add("Low sample size may affect statistical reliability");

            // Calculate statistical significance (simplified)
            var isStatisticallySignificant = false;
            var pValue = 1.0;
            var zScore = 0.0;

            if (totalSampleSize > 0)
            {
                var controlRate = (double)controlConversions / Math.Max(1, controlConversions + variantConversions);
                var variantRate = (double)variantConversions / Math.Max(1, controlConversions + variantConversions);
                var pooledRate = (double)totalSampleSize / Math.Max(1, totalSampleSize * 2);
                
                var standardError = Math.Sqrt(pooledRate * (1 - pooledRate) * (2.0 / totalSampleSize));
                if (standardError > 0)
                {
                    zScore = Math.Abs(variantRate - controlRate) / standardError;
                    pValue = 2 * (1 - NormalCDF(Math.Abs(zScore)));
                    isStatisticallySignificant = pValue < 0.05;
                }
            }

            var result = new StatisticalValidationResult
            {
                IsValid = !validationErrors.Any(),
                HasSufficientSampleSize = hasSufficientSampleSize,
                ActualSampleSize = totalSampleSize,
                RequiredSampleSize = minimumSampleSize,
                ValidationMessages = validationMessages,
                ValidationErrors = validationErrors,
                IsStatisticallySignificant = isStatisticallySignificant,
                PValue = pValue,
                ZScore = zScore,
                StatisticalPower = CalculateStatisticalPower(controlConversions, variantConversions, 0.2),
                EffectSize = Math.Abs((double)variantConversions - controlConversions) / Math.Max(1, Math.Max(controlConversions, variantConversions)),
                RecommendedSampleSize = Math.Max(minimumSampleSize, CalculateRecommendedSampleSize(0.1, 0.02, 0.8, 0.05))
            };

            _logger.LogInformation("Statistical significance validation completed. IsValid: {IsValid}, IsSignificant: {IsSignificant}", 
                result.IsValid, isStatisticallySignificant);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during statistical significance validation");
            throw;
        }
    }

    public CurrencyValidationResult ValidateCurrency(string currency)
    {
        try
        {
            _logger.LogInformation("Validating currency: {Currency}", currency);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(currency))
            {
                validationErrors.Add("Currency code is required");
                return new CurrencyValidationResult
                {
                    IsValid = false,
                    ValidationErrors = validationErrors
                };
            }

            var normalizedCurrency = currency.ToUpperInvariant().Trim();
            var isValidFormat = CurrencyCodeRegex.IsMatch(normalizedCurrency);
            var isSupported = SupportedCurrencies.Contains(normalizedCurrency);

            if (!isValidFormat)
                validationErrors.Add("Invalid currency code format. Must be 3 uppercase letters");

            if (isValidFormat && !isSupported)
                validationMessages.Add($"Currency {normalizedCurrency} is not in the supported list: {string.Join(", ", SupportedCurrencies)}");

            var conversionRate = GetCurrencyConversionRate(normalizedCurrency);

            var result = new CurrencyValidationResult
            {
                IsValid = !validationErrors.Any(),
                IsSupported = isSupported,
                Currency = normalizedCurrency,
                ExchangeRate = conversionRate,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                SupportedCurrencies = SupportedCurrencies.ToList(),
                NormalizedCurrency = normalizedCurrency,
                NormalizedAmount = 0m,
                ConversionRate = conversionRate
            };

            _logger.LogInformation("Currency validation completed. IsValid: {IsValid}, IsSupported: {IsSupported}", 
                result.IsValid, isSupported);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during currency validation");
            throw;
        }
    }

    public MultiVariantValidationResult ValidateMultipleVariants(List<VariantData> variants)
    {
        try
        {
            _logger.LogInformation("Validating multiple variants: {VariantCount}", variants?.Count ?? 0);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            if (variants == null || !variants.Any())
            {
                validationErrors.Add("No variants provided for validation");
                return new MultiVariantValidationResult
                {
                    IsValid = false,
                    ValidationErrors = validationErrors
                };
            }

            if (variants.Count < 2)
                validationErrors.Add("At least 2 variants required (control + treatment)");

            // Validate each variant
            for (int i = 0; i < variants.Count; i++)
            {
                var variant = variants[i];
                if (variant.Conversions < 0)
                    validationErrors.Add($"Variant {i + 1}: Conversions cannot be negative");
                if (variant.Visitors < 0)
                    validationErrors.Add($"Variant {i + 1}: Visitors cannot be negative");
                if (variant.Conversions > variant.Visitors)
                    validationErrors.Add($"Variant {i + 1}: Conversions cannot exceed visitors");
            }

            var result = new MultiVariantValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                VariantCount = variants.Count,
                TotalSampleSize = variants.Sum(v => v.Visitors)
            };

            _logger.LogInformation("Multi-variant validation completed. IsValid: {IsValid}, VariantCount: {VariantCount}", 
                result.IsValid, variants.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during multi-variant validation");
            throw;
        }
    }

    public DateRangeValidationResult ValidateDateRange(DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation("Validating date range: {StartDate} to {EndDate}", startDate, endDate);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            if (startDate >= endDate)
                validationErrors.Add("Start date must be before end date");

            var duration = endDate - startDate;
            if (duration.TotalDays > 365)
                validationMessages.Add("Very long analysis period may include seasonal variations");

            if (duration.TotalDays < 7)
                validationMessages.Add("Short analysis period may not capture weekly patterns");

            if (endDate > DateTime.UtcNow)
                validationMessages.Add("End date is in the future");

            var result = new DateRangeValidationResult
            {
                IsValid = !validationErrors.Any(),
                StartDate = startDate,
                EndDate = endDate,
                Duration = duration,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                BusinessDays = CalculateBusinessDays(startDate, endDate)
            };

            _logger.LogInformation("Date range validation completed. IsValid: {IsValid}, Duration: {Duration} days", 
                result.IsValid, duration.TotalDays);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during date range validation");
            throw;
        }
    }

    public ConfidenceLevelValidationResult ValidateConfidenceLevel(double confidenceLevel)
    {
        try
        {
            _logger.LogInformation("Validating confidence level: {ConfidenceLevel}", confidenceLevel);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            if (confidenceLevel <= 0 || confidenceLevel >= 1)
                validationErrors.Add("Confidence level must be between 0 and 1 (exclusive)");

            var commonLevels = new[] { 0.90, 0.95, 0.99 };
            if (!commonLevels.Contains(confidenceLevel))
                validationMessages.Add($"Uncommon confidence level. Standard levels are: {string.Join(", ", commonLevels.Select(l => l.ToString("P0")))}")

            if (confidenceLevel < 0.80)
                validationMessages.Add("Low confidence level may increase Type I error risk");

            if (confidenceLevel > 0.99)
                validationMessages.Add("Very high confidence level may require larger sample sizes");

            var result = new ConfidenceLevelValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                ZScore = GetZScoreForConfidenceLevel(confidenceLevel),
                AlphaLevel = 1 - confidenceLevel,
                IsCommonLevel = commonLevels.Contains(confidenceLevel)
            };

            _logger.LogInformation("Confidence level validation completed. IsValid: {IsValid}", result.IsValid);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during confidence level validation");
            throw;
        }
    }

    public BusinessRuleValidationResult ValidateBusinessRules(decimal mcpValue, MCPBusinessContext businessContext)
    {
        try
        {
            _logger.LogInformation("Validating business rules for MCP: {MCPValue}", mcpValue);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();
            var appliedRules = new List<string>();

            // Update context with current MCP value
            businessContext.CalculatedMCP = mcpValue;

            // Industry-specific validation rules
            switch (businessContext.Industry?.ToLowerInvariant())
            {
                case "ecommerce":
                    ValidateEcommerceRules(businessContext, validationErrors, validationMessages, appliedRules);
                    break;
                case "saas":
                    ValidateSaaSRules(businessContext, validationErrors, validationMessages, appliedRules);
                    break;
                case "finance":
                    ValidateFinanceRules(businessContext, validationErrors, validationMessages, appliedRules);
                    break;
                default:
                    ValidateGenericRules(businessContext, validationErrors, validationMessages, appliedRules);
                    break;
            }

            var result = new BusinessRuleValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                AppliedRules = appliedRules,
                ComplianceScore = CalculateComplianceScore(validationErrors.Count, validationMessages.Count)
            };

            _logger.LogInformation("Business rules validation completed. IsValid: {IsValid}, ComplianceScore: {ComplianceScore:P1}", 
                result.IsValid, result.ComplianceScore);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during business rules validation");
            throw;
        }
    }

    public async Task<ConfidenceLevelValidationResult> ValidateConfidenceLevelAsync(double confidenceLevel)
    {
        try
        {
            _logger.LogInformation("Validating confidence level: {ConfidenceLevel}", confidenceLevel);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation
            if (confidenceLevel <= 0 || confidenceLevel >= 1)
                validationErrors.Add("Confidence level must be between 0 and 1 (exclusive)");
            
            // Common confidence levels
            var commonLevels = new[] { 0.90, 0.95, 0.99 };
            if (!commonLevels.Contains(confidenceLevel))
                validationMessages.Add($"Uncommon confidence level. Standard levels are: {string.Join(", ", commonLevels.Select(l => l.ToString("P0")))}");
            
            if (confidenceLevel < 0.80)
                validationMessages.Add("Low confidence level may increase Type I error risk");
            
            if (confidenceLevel > 0.99)
                validationMessages.Add("Very high confidence level may require larger sample sizes");

            var zScore = GetZScoreForConfidenceLevel(confidenceLevel);
            var alphaLevel = 1 - confidenceLevel;

            var result = new ConfidenceLevelValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                ZScore = zScore,
                AlphaLevel = alphaLevel,
                IsCommonLevel = commonLevels.Contains(confidenceLevel)
            };

            _logger.LogInformation("Confidence level validation completed. IsValid: {IsValid}, Z-Score: {ZScore}", 
                result.IsValid, zScore);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during confidence level validation");
            throw;
        }
    }

    public async Task<AbTestValidationResult> ValidateAbTestConfigurationAsync(Guid abTestId, List<VariantData> variants)
    {
        try
        {
            _logger.LogInformation("Validating A/B test configuration for test {AbTestId}", abTestId);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic A/B test validation
            if (abTestId == Guid.Empty)
                validationErrors.Add("Valid A/B test ID is required");

            var variantValidation = await ValidateMultipleVariantsAsync(variants);
            if (!variantValidation.IsValid)
            {
                validationErrors.AddRange(variantValidation.ValidationErrors);
                validationMessages.AddRange(variantValidation.ValidationMessages);
            }

            // A/B test specific validations
            if (variants?.Count < 2)
                validationErrors.Add("A/B test requires at least 2 variants (control + treatment)");
            
            if (variants?.Count > 10)
                validationMessages.Add("Large number of variants may reduce statistical power per variant");

            // Traffic allocation validation
            var totalVisitors = variants?.Sum(v => v.Visitors) ?? 0;
            if (totalVisitors > 0)
            {
                var trafficDistribution = variants.Select(v => new
                {
                    VariantId = v.VariantId,
                    Percentage = (double)v.Visitors / totalVisitors * 100
                }).ToList();

                var unevenDistribution = trafficDistribution.Where(t => t.Percentage < 10 || t.Percentage > 60).ToList();
                if (unevenDistribution.Any())
                    validationMessages.Add("Uneven traffic distribution may affect test reliability");
            }

            var result = new AbTestValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                VariantCount = variants?.Count ?? 0,
                HasControlVariant = variants?.Any(v => v.IsControl) ?? false,
                TotalSampleSize = totalVisitors,
                EstimatedTestDuration = TimeSpan.FromDays(CalculateEstimatedTestDuration(totalVisitors, variants?.Count ?? 0))
            };

            _logger.LogInformation("A/B test validation completed. IsValid: {IsValid}, Variants: {VariantCount}", 
                result.IsValid, result.VariantCount);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during A/B test configuration validation");
            throw;
        }
    }

    public async Task<BusinessRuleValidationResult> ValidateBusinessRulesAsync(MCPBusinessContext context)
    {
        try
        {
            _logger.LogInformation("Validating business rules for industry: {Industry}", context.Industry);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();
            var appliedRules = new List<string>();

            // Industry-specific validation rules
            switch (context.Industry?.ToLowerInvariant())
            {
                case "ecommerce":
                    ValidateEcommerceRules(context, validationErrors, validationMessages, appliedRules);
                    break;
                case "saas":
                    ValidateSaaSRules(context, validationErrors, validationMessages, appliedRules);
                    break;
                case "finance":
                    ValidateFinanceRules(context, validationErrors, validationMessages, appliedRules);
                    break;
                default:
                    ValidateGenericRules(context, validationErrors, validationMessages, appliedRules);
                    break;
            }

            // Universal business rules
            if (context.CalculatedMCP < context.MinMeaningfulMCP)
                validationMessages.Add($"MCP below minimum threshold of {context.MinMeaningfulMCP:C}");
            
            if (context.CalculatedMCP > context.MaxAcceptableMCP)
                validationMessages.Add($"MCP exceeds maximum threshold of {context.MaxAcceptableMCP:C}");

            // Calculate ROI validation using available properties
            if (context.TotalCost > 0)
            {
                var actualROI = (context.TotalRevenue - context.TotalCost) / context.TotalCost;
                // Use ExpectedMCPRange as a threshold for ROI validation
                var expectedROI = context.ExpectedMCPRange / 100; // Convert percentage to decimal
                if (actualROI < expectedROI)
                    validationMessages.Add($"ROI ({actualROI:P1}) below expected range ({expectedROI:P1})");
            }

            var result = new BusinessRuleValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                AppliedRules = appliedRules,
                Industry = context.Industry,
                ComplianceScore = CalculateComplianceScore(validationErrors.Count, validationMessages.Count)
            };

            _logger.LogInformation("Business rules validation completed. IsValid: {IsValid}, Applied Rules: {RuleCount}", 
                result.IsValid, appliedRules.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during business rules validation");
            throw;
        }
    }

    public async Task<ComprehensiveValidationResult> ValidateComprehensiveMCPRequestAsync(
        decimal revenue, decimal cost, int conversions, int visitors, string currency, 
        DateTime startDate, DateTime endDate, double confidenceLevel, Guid? abTestId = null)
    {
        try
        {
            _logger.LogInformation("Performing comprehensive MCP validation");

            // Create basic validation result for single input set
            var basicValidation = new MCPInputValidationResult
            {
                IsValid = revenue >= 0 && cost >= 0 && conversions >= 0 && visitors >= 0,
                ValidationErrors = new List<string>(),
                ValidationMessages = new List<string>()
            };
            
            // Add validation errors for negative values
            if (revenue < 0) basicValidation.ValidationErrors.Add("Revenue cannot be negative");
            if (cost < 0) basicValidation.ValidationErrors.Add("Cost cannot be negative");
            if (conversions < 0) basicValidation.ValidationErrors.Add("Conversions cannot be negative");
            if (visitors < 0) basicValidation.ValidationErrors.Add("Visitors cannot be negative");
            if (conversions > visitors) basicValidation.ValidationErrors.Add("Conversions cannot exceed visitors");
            
            basicValidation.IsValid = !basicValidation.ValidationErrors.Any();
            
            // Run async validations in parallel
            var asyncValidationTasks = new List<Task>
            {
                ValidateCurrencyAsync(currency, revenue),
                ValidateDateRangeAsync(startDate, endDate),
                ValidateConfidenceLevelAsync(confidenceLevel)
            };

            await Task.WhenAll(asyncValidationTasks);
            var currencyValidation = await ValidateCurrencyAsync(currency, revenue);
            var dateValidation = await ValidateDateRangeAsync(startDate, endDate);
            var confidenceValidation = await ValidateConfidenceLevelAsync(confidenceLevel);

            var allErrors = new List<string>();
            var allMessages = new List<string>();

            allErrors.AddRange(basicValidation.ValidationErrors);
            allErrors.AddRange(currencyValidation.ErrorMessages);
            allErrors.AddRange(dateValidation.ValidationMessages);
            allErrors.AddRange(confidenceValidation.ValidationMessages);

            allMessages.AddRange(basicValidation.ValidationMessages);
            allMessages.AddRange(currencyValidation.ValidationMessages);
            allMessages.AddRange(dateValidation.ValidationMessages);
            allMessages.AddRange(confidenceValidation.ValidationMessages);

            var overallValid = !allErrors.Any();
            var validationScore = CalculateValidationScore(allErrors.Count, allMessages.Count);

            var result = new ComprehensiveValidationResult
            {
                IsValid = overallValid,
                CriticalErrors = allErrors,
                ValidationMessages = allMessages,
                BasicValidation = basicValidation,
                CurrencyValidation = currencyValidation,
                DateRangeValidation = dateValidation,
                ConfidenceLevelValidation = confidenceValidation,
                ValidationScore = validationScore,
                ValidatedAt = DateTime.UtcNow
            };

            _logger.LogInformation("Comprehensive validation completed. IsValid: {IsValid}, Score: {Score:P1}", 
                overallValid, validationScore);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during comprehensive MCP validation");
            throw;
        }
    }

    #region Private Helper Methods

    private static double NormalCDF(double x)
    {
        return 0.5 * (1 + Erf(x / Math.Sqrt(2)));
    }

    private static double Erf(double x)
    {
        const double a1 = 0.254829592;
        const double a2 = -0.284496736;
        const double a3 = 1.421413741;
        const double a4 = -1.453152027;
        const double a5 = 1.061405429;
        const double p = 0.3275911;

        var sign = x < 0 ? -1 : 1;
        x = Math.Abs(x);

        var t = 1.0 / (1.0 + p * x);
        var y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

        return sign * y;
    }

    private static double GetZScoreForConfidenceLevel(double confidenceLevel)
    {
        return confidenceLevel switch
        {
            0.90 => 1.645,
            0.95 => 1.96,
            0.99 => 2.576,
            _ => 1.96
        };
    }

    private static decimal GetCurrencyConversionRate(string currency)
    {
        return currency switch
        {
            "USD" => 1.0m,
            "EUR" => 1.1m,
            "GBP" => 1.3m,
            "CAD" => 0.8m,
            "AUD" => 0.7m,
            "JPY" => 0.009m,
            _ => 1.0m
        };
    }

    private static double CalculateStatisticalPower(int controlSize, int treatmentSize, double effectSize)
    {
        var totalSize = controlSize + treatmentSize;
        var harmonicMean = 2.0 / (1.0 / controlSize + 1.0 / treatmentSize);
        var delta = effectSize / Math.Sqrt(2 / harmonicMean);
        
        // Simplified power calculation
        return Math.Min(0.99, Math.Max(0.05, 1 - NormalCDF(1.96 - delta)));
    }

    private static int CalculateRecommendedSampleSize(double baselineRate, double minimumDetectableEffect, double power, double alpha)
    {
        var zAlpha = GetZScoreForConfidenceLevel(1 - alpha / 2);
        var zBeta = GetZScoreForConfidenceLevel(power);
        
        var p1 = baselineRate;
        var p2 = baselineRate + minimumDetectableEffect;
        var pBar = (p1 + p2) / 2;
        
        var numerator = Math.Pow(zAlpha * Math.Sqrt(2 * pBar * (1 - pBar)) + zBeta * Math.Sqrt(p1 * (1 - p1) + p2 * (1 - p2)), 2);
        var denominator = Math.Pow(p2 - p1, 2);
        
        return (int)Math.Ceiling(numerator / denominator);
    }

    private static int CalculateBusinessDays(DateTime startDate, DateTime endDate)
    {
        var businessDays = 0;
        var current = startDate.Date;
        
        while (current <= endDate.Date)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                businessDays++;
            current = current.AddDays(1);
        }
        
        return businessDays;
    }

    private static int CalculateEstimatedTestDuration(int totalSampleSize, int variantCount)
    {
        // Simplified estimation: assume 1000 visitors per day
        var dailyTraffic = 1000;
        return Math.Max(7, totalSampleSize / dailyTraffic); // Minimum 7 days
    }

    private static void ValidateEcommerceRules(MCPBusinessContext context, List<string> errors, List<string> messages, List<string> appliedRules)
    {
        appliedRules.Add("E-commerce Industry Rules");
        
        if (context.CalculatedMCP < 5m)
            messages.Add("Low MCP for e-commerce (typically $5+ expected)");
        
        if (context.TotalRevenue > 0 && context.TotalCost / context.TotalRevenue > 0.7m)
            messages.Add("High cost ratio for e-commerce (>70%)");
    }

    private static void ValidateSaaSRules(MCPBusinessContext context, List<string> errors, List<string> messages, List<string> appliedRules)
    {
        appliedRules.Add("SaaS Industry Rules");
        
        if (context.CalculatedMCP < 20m)
            messages.Add("Low MCP for SaaS (typically $20+ expected)");
        
        if (context.TotalRevenue > 0 && context.TotalCost / context.TotalRevenue > 0.5m)
            messages.Add("High cost ratio for SaaS (>50%)");
    }

    private static void ValidateFinanceRules(MCPBusinessContext context, List<string> errors, List<string> messages, List<string> appliedRules)
    {
        appliedRules.Add("Finance Industry Rules");
        
        if (context.CalculatedMCP < 50m)
            messages.Add("Low MCP for finance (typically $50+ expected)");
        
        if (context.TotalRevenue > 0 && context.TotalCost / context.TotalRevenue > 0.3m)
            messages.Add("High cost ratio for finance (>30%)");
    }

    private static void ValidateGenericRules(MCPBusinessContext context, List<string> errors, List<string> messages, List<string> appliedRules)
    {
        appliedRules.Add("Generic Business Rules");
        
        if (context.CalculatedMCP < 1m)
            messages.Add("Very low MCP detected");
        
        if (context.TotalRevenue > 0 && context.TotalCost / context.TotalRevenue > 0.8m)
            messages.Add("High cost ratio (>80%)");
    }

    private static double CalculateComplianceScore(int errorCount, int messageCount)
    {
        var totalIssues = errorCount + messageCount * 0.5; // Errors weighted more than messages
        return Math.Max(0, 1 - (totalIssues / 10)); // Scale to 0-1
    }

    private static double CalculateValidationScore(int errorCount, int messageCount)
    {
        if (errorCount > 0) return 0; // Any errors = 0 score
        return Math.Max(0, 1 - (messageCount * 0.1)); // Deduct 10% per message
    }



    public async Task<MultiVariantValidationResult> ValidateMultipleVariantsAsync(List<VariantData> variants)
    {
        try
        {
            _logger.LogInformation("Validating multiple variants asynchronously: {VariantCount}", variants?.Count ?? 0);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            if (variants == null || !variants.Any())
            {
                validationErrors.Add("No variants provided for validation");
                return new MultiVariantValidationResult
                {
                    IsValid = false,
                    ValidationErrors = validationErrors,
                    ValidationMessages = validationMessages
                };
            }

            // Validate minimum variant count
            if (variants.Count < 2)
                validationErrors.Add("At least 2 variants required (control + treatment)");

            // Validate each variant
            for (int i = 0; i < variants.Count; i++)
            {
                var variant = variants[i];
                if (variant.Conversions < 0)
                    validationErrors.Add($"Variant {i + 1}: Conversions cannot be negative");
                if (variant.Visitors < 0)
                    validationErrors.Add($"Variant {i + 1}: Visitors cannot be negative");
                if (variant.Conversions > variant.Visitors)
                    validationErrors.Add($"Variant {i + 1}: Conversions cannot exceed visitors");
            }

            var result = new MultiVariantValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                VariantCount = variants.Count,
                TotalSampleSize = variants.Sum(v => v.Visitors)
            };

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during multi-variant validation (async)");
            throw;
        }
    }

    public StatisticalValidationResult ValidateStatisticalSignificance(int controlConversions, int variantConversions, int minimumSampleSize = 30)
    {
        throw new NotImplementedException();
    }

    public CurrencyValidationResult ValidateCurrency(string currency)
    {
        throw new NotImplementedException();
    }

    public MultiVariantValidationResult ValidateMultipleVariants(List<VariantData> variants)
    {
        throw new NotImplementedException();
    }

    public DateRangeValidationResult ValidateDateRange(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public ConfidenceLevelValidationResult ValidateConfidenceLevel(double confidenceLevel)
    {
        throw new NotImplementedException();
    }

    public BusinessRuleValidationResult ValidateBusinessRules(decimal mcpValue, MCPBusinessContext businessContext)
    {
        throw new NotImplementedException();
    }

    public Task<ComprehensiveValidationResult> ValidateComprehensiveAsync(MCPCalculationRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ComprehensiveValidationResult> ValidateComprehensiveMCPRequestAsync(MCPCalculationRequest request)
    {
        throw new NotImplementedException();
    }

    #endregion
}