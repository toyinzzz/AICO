namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Confidence level validation result
/// </summary>
public class ConfidenceLevelValidationResult
{
    public bool IsValid { get; set; }
    public double ConfidenceLevel { get; set; }
    public bool IsStandardLevel { get; set; }
    public double AlphaLevel { get; set; }
    public bool IsCommonLevel { get; set; }
    public double ZScore { get; set; }
    public double RecommendedLevel { get; set; }
    public List<double> RecommendedLevels { get; set; } = new() { 0.90, 0.95, 0.99 };
    public List<string> ValidationMessages { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
}