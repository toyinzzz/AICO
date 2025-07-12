using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs.ValidationResults;
using AICO.Domain.DTOs.Requests;
using AICO.Application.DTOs;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.Common;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Main MCP validation service that orchestrates validation using specialized services
/// Follows Single Responsibility Principle by delegating to focused validation services
/// </summary>
public class MCPValidationService : IMCPValidationService
{
    private readonly ILogger<MCPValidationService> _logger;
    private readonly IBasicInputValidationService _basicInputValidationService;
    private readonly IStatisticalValidationService _statisticalValidationService;
    private readonly ICurrencyValidationService _currencyValidationService;
    private readonly IBusinessRuleValidationService _businessRuleValidationService;

    public MCPValidationService(
        ILogger<MCPValidationService> logger,
        IBasicInputValidationService basicInputValidationService,
        IStatisticalValidationService statisticalValidationService,
        ICurrencyValidationService currencyValidationService,
        IBusinessRuleValidationService businessRuleValidationService)
    {
        _logger = logger;
        _basicInputValidationService = basicInputValidationService;
        _statisticalValidationService = statisticalValidationService;
        _currencyValidationService = currencyValidationService;
        _businessRuleValidationService = businessRuleValidationService;
    }

    // Delegate to BasicInputValidationService
    public MCPInputValidationResult ValidateBasicInputs(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
    {
        _logger.LogInformation("Delegating basic input validation to specialized service");
        return _basicInputValidationService.ValidateBasicInputs(controlRevenue, controlConversions, variantRevenue, variantConversions);
    }

    public async Task<MCPInputValidationResult> ValidateBasicInputsAsync(decimal revenue, decimal cost, int conversions, int visitors)
    {
        _logger.LogInformation("Delegating async basic input validation to specialized service");
        return await _basicInputValidationService.ValidateBasicInputsAsync(revenue, cost, conversions, visitors);
    }

    public MultiVariantValidationResult ValidateMultipleVariants(List<VariantData> variants)
    {
        _logger.LogInformation("Delegating multiple variants validation to specialized service");
        return _basicInputValidationService.ValidateMultipleVariants(variants);
    }

    public async Task<MultiVariantValidationResult> ValidateMultipleVariantsAsync(List<VariantData> variants)
    {
        _logger.LogInformation("Delegating async multiple variants validation to specialized service");
        return await _basicInputValidationService.ValidateMultipleVariantsAsync(variants);
    }

    // Delegate to StatisticalValidationService
    public StatisticalValidationResult ValidateStatisticalSignificance(int controlConversions, int variantConversions, int minimumSampleSize = 30)
    {
        _logger.LogInformation("Delegating statistical significance validation to specialized service");
        return _statisticalValidationService.ValidateStatisticalSignificance(controlConversions, variantConversions, minimumSampleSize);
    }

    public async Task<StatisticalValidationResult> ValidateStatisticalSignificanceAsync(int controlConversions, int controlVisitors, int treatmentConversions, int treatmentVisitors, double confidenceLevel = 0.95)
    {
        _logger.LogInformation("Delegating async statistical significance validation to specialized service");
        return await _statisticalValidationService.ValidateStatisticalSignificanceAsync(controlConversions, controlVisitors, treatmentConversions, treatmentVisitors, confidenceLevel);
    }

    public ConfidenceLevelValidationResult ValidateConfidenceLevel(double confidenceLevel)
    {
        _logger.LogInformation("Delegating confidence level validation to specialized service");
        return _statisticalValidationService.ValidateConfidenceLevel(confidenceLevel);
    }

    public async Task<ConfidenceLevelValidationResult> ValidateConfidenceLevelAsync(double confidenceLevel)
    {
        _logger.LogInformation("Delegating async confidence level validation to specialized service");
        return await _statisticalValidationService.ValidateConfidenceLevelAsync(confidenceLevel);
    }

    // Delegate to CurrencyValidationService
    public CurrencyValidationResult ValidateCurrency(string currency)
    {
        _logger.LogInformation("Delegating currency validation to specialized service");
        return _currencyValidationService.ValidateCurrency(currency);
    }

    public async Task<CurrencyValidationResult> ValidateCurrencyAsync(List<(decimal amount, string currency)> amounts, string targetCurrency)
    {
        _logger.LogInformation("Delegating async currency validation to specialized service");
        return await _currencyValidationService.ValidateCurrencyAsync(amounts, targetCurrency);
    }

    // Delegate to BusinessRuleValidationService
    public BusinessRuleValidationResult ValidateBusinessRules(decimal mcpValue, MCPBusinessContext businessContext)
    {
        _logger.LogInformation("Delegating business rules validation to specialized service");
        return _businessRuleValidationService.ValidateBusinessRules(mcpValue, businessContext);
    }

    public DateRangeValidationResult ValidateDateRange(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Delegating date range validation to specialized service");
        return _businessRuleValidationService.ValidateDateRange(startDate, endDate);
    }

    public async Task<DateRangeValidationResult> ValidateDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Delegating async date range validation to specialized service");
        return await _businessRuleValidationService.ValidateDateRangeAsync(startDate, endDate);
    }

    public async Task<AbTestValidationResult> ValidateAbTestConfigurationAsync(Guid abTestId, List<VariantData> variants)
    {
        _logger.LogInformation("Delegating A/B test configuration validation to specialized service");
        return await _businessRuleValidationService.ValidateAbTestConfigurationAsync(abTestId, variants);
    }

    public async Task<ComprehensiveValidationResult> ValidateComprehensiveMCPRequestAsync(MCPCalculationRequest request)
    {
        _logger.LogInformation("Delegating comprehensive MCP request validation to specialized service");
        return await _businessRuleValidationService.ValidateComprehensiveMCPRequestAsync(request);
    }
}