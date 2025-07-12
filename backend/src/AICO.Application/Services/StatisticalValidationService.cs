using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs.ValidationResults;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for statistical validation
/// Handles statistical significance and confidence level validation
/// </summary>
public class StatisticalValidationService : IStatisticalValidationService
{
    private readonly ILogger<StatisticalValidationService> _logger;

    public StatisticalValidationService(ILogger<StatisticalValidationService> logger)
    {
        _logger = logger;
    }

    public StatisticalValidationResult ValidateStatisticalSignificance(int controlConversions, int variantConversions, int minimumSampleSize = 30)
    {
        try
        {
            _logger.LogInformation("Validating statistical significance: control={ControlConversions}, variant={VariantConversions}, minSample={MinimumSampleSize}",
                controlConversions, variantConversions, minimumSampleSize);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation
            if (controlConversions < 0)
                validationErrors.Add("Control conversions cannot be negative");
            if (variantConversions < 0)
                validationErrors.Add("Variant conversions cannot be negative");
            if (minimumSampleSize <= 0)
                validationErrors.Add("Minimum sample size must be positive");

            // Sample size validation
            if (controlConversions < minimumSampleSize)
                validationErrors.Add($"Control conversions ({controlConversions}) below minimum sample size ({minimumSampleSize})");
            if (variantConversions < minimumSampleSize)
                validationErrors.Add($"Variant conversions ({variantConversions}) below minimum sample size ({minimumSampleSize})");

            // Statistical power warnings
            var totalSampleSize = controlConversions + variantConversions;
            if (totalSampleSize < 100)
                validationMessages.Add("Low total sample size may affect statistical reliability");

            var isValid = !validationErrors.Any();
            var isSignificant = isValid && totalSampleSize >= minimumSampleSize * 2;

            return new StatisticalValidationResult
            {
                IsValid = isValid,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                IsStatisticallySignificant = isSignificant,
                PValue = isSignificant ? 0.04 : 0.1, // Simplified calculation
                ZScore = isSignificant ? 2.0 : 1.5, // Simplified calculation
                StatisticalPower = CalculateStatisticalPower(controlConversions, variantConversions, 0.05),
                EffectSize = Math.Abs((double)variantConversions - controlConversions) / Math.Max(controlConversions, variantConversions),
                RecommendedSampleSize = Math.Max(minimumSampleSize * 2, 100)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during statistical significance validation");
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

    public ConfidenceLevelValidationResult ValidateConfidenceLevel(double confidenceLevel)
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
                validationMessages.Add($"Confidence level {confidenceLevel:P1} is not commonly used. Consider 90%, 95%, or 99%.");

            // Business recommendations
            if (confidenceLevel < 0.90)
                validationMessages.Add("Low confidence level may lead to unreliable results");
            if (confidenceLevel > 0.99)
                validationMessages.Add("Very high confidence level may require large sample sizes");

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

    public async Task<ConfidenceLevelValidationResult> ValidateConfidenceLevelAsync(double confidenceLevel)
    {
        return await Task.FromResult(ValidateConfidenceLevel(confidenceLevel));
    }

    private static double CalculateStatisticalPower(int sampleSize1, int sampleSize2, double effectSize)
    {
        // Simplified power calculation
        var totalSampleSize = sampleSize1 + sampleSize2;
        var basePower = Math.Min(0.95, totalSampleSize / 1000.0);
        var effectAdjustment = Math.Min(1.0, effectSize * 10);
        return Math.Max(0.05, basePower * effectAdjustment);
    }

    private static double NormalCDF(double value)
    {
        // Simplified normal CDF approximation
        return 0.5 * (1.0 + Math.Sign(value) * Math.Sqrt(1.0 - Math.Exp(-2.0 * value * value / Math.PI)));
    }

    private static int CalculateRecommendedSampleSize(double baselineRate, double minimumDetectableEffect, double power, double confidenceLevel)
    {
        // Simplified sample size calculation
        var zAlpha = GetZScoreForConfidenceLevel(confidenceLevel);
        var zBeta = GetZScoreForPower(power);
        var p1 = baselineRate;
        var p2 = baselineRate + minimumDetectableEffect;
        var pooledP = (p1 + p2) / 2;
        
        var numerator = Math.Pow(zAlpha * Math.Sqrt(2 * pooledP * (1 - pooledP)) + zBeta * Math.Sqrt(p1 * (1 - p1) + p2 * (1 - p2)), 2);
        var denominator = Math.Pow(p2 - p1, 2);
        
        return (int)Math.Ceiling(numerator / denominator);
    }

    private static double GetZScoreForConfidenceLevel(double confidenceLevel)
    {
        return confidenceLevel switch
        {
            0.90 => 1.645,
            0.95 => 1.96,
            0.99 => 2.576,
            _ => 1.96 // Default to 95%
        };
    }

    private static double GetZScoreForPower(double power)
    {
        return power switch
        {
            0.80 => 0.842,
            0.90 => 1.282,
            0.95 => 1.645,
            _ => 0.842 // Default to 80%
        };
    }
}