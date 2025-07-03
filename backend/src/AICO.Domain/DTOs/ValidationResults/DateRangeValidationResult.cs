namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Date range validation result
/// </summary>
public class DateRangeValidationResult
{
    public bool IsValid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsReasonableDuration { get; set; }
    public bool IsInFuture { get; set; }
    public bool IsCurrentPeriod { get; set; }
    public int DurationDays { get; set; }
    public int BusinessDays { get; set; }
    public bool IncludesWeekends { get; set; }
    public List<string> ValidationMessages { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
    public string RecommendedDateRange { get; set; } = string.Empty;
}