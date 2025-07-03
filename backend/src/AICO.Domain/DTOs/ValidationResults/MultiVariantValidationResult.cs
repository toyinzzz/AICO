namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Individual variant validation error
/// </summary>
public class VariantValidationError
{
    public Guid VariantId { get; set; }
    public int VariantIndex { get; set; }
    public string ErrorType { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// Multi-variant validation result
/// </summary>
public class MultiVariantValidationResult
{
    public bool IsValid { get; set; }
    public bool HasControlVariant { get; set; }
    public int VariantCount { get; set; }
    public int ValidVariantCount { get; set; }
    public List<VariantValidationError> VariantErrors { get; set; } = new();
    public List<string> GeneralErrors { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ValidationMessages { get; set; } = new();
    public int TotalVariants { get; set; }
    public int ControlVariantCount { get; set; }
    public int TotalSampleSize { get; set; }
    public string RecommendedAction { get; set; } = string.Empty;
}