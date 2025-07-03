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
}