using System.Collections.Generic;

namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Currency validation result
/// </summary>
public class CurrencyValidationResult
{
    public bool IsValid { get; set; }
    public bool IsSupported { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal? ExchangeRate { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> SupportedCurrencies { get; set; } = new();
    
    // Additional properties needed by MCPValidationService
    public List<string> ValidationMessages { get; set; } = new();
    public string NormalizedCurrency { get; set; } = string.Empty;
    public decimal NormalizedAmount { get; set; }
    public decimal ConversionRate { get; set; }
    
    // Properties for multi-currency validation
    public string TargetCurrency { get; set; } = string.Empty;
    public Dictionary<string, decimal> ConversionRates { get; set; } = new();
    public List<(decimal Amount, string Currency)> NormalizedAmounts { get; set; } = new();
}