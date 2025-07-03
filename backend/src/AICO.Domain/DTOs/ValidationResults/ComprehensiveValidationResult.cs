namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Comprehensive validation result combining all validation aspects
/// </summary>
public class ComprehensiveValidationResult
{
    public bool IsValid { get; set; }
    public MCPInputValidationResult BasicValidation { get; set; } = new();
    public StatisticalValidationResult StatisticalValidation { get; set; } = new();
    public CurrencyValidationResult? CurrencyValidation { get; set; }
    public DateRangeValidationResult? DateRangeValidation { get; set; }
    public BusinessRuleValidationResult BusinessRuleValidation { get; set; } = new();
    public double ValidationScore { get; set; }
    public DateTime ValidatedAt { get; set; }
    public List<string> CriticalErrors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public List<string> ValidationMessages { get; set; } = new();
    public ConfidenceLevelValidationResult ConfidenceLevelValidation { get; set; } = new();
    public string OverallRiskAssessment { get; set; } = "Low";
    public bool CanProceedWithCalculation { get; set; }
}