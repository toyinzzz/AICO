namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// A/B test validation result
/// </summary>
public class AbTestValidationResult
{
    public bool IsValid { get; set; }
    public Guid AbTestId { get; set; }
    public bool TestExists { get; set; }
    public bool IsActive { get; set; }
    public bool HasSufficientData { get; set; }
    public int TotalParticipants { get; set; }
    public int VariantCount { get; set; }
    public bool HasControlVariant { get; set; }
    public int TotalSampleSize { get; set; }
    public TimeSpan EstimatedTestDuration { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ValidationMessages { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public string TestStatus { get; set; } = string.Empty;
}