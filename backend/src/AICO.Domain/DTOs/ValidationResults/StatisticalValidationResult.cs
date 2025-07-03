namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Statistical validation result for MCP calculations
/// </summary>
public class StatisticalValidationResult
{
    public bool IsValid { get; set; }
    public bool HasSufficientSampleSize { get; set; }
    public int ActualSampleSize { get; set; }
    public int RequiredSampleSize { get; set; }
    public double PowerAnalysis { get; set; }
    public List<string> ValidationMessages { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
    public string RecommendedAction { get; set; } = string.Empty;
    public bool IsStatisticallySignificant { get; set; }
    public double PValue { get; set; }
    public double ZScore { get; set; }
    public double StatisticalPower { get; set; }
    public double EffectSize { get; set; }
    public int RecommendedSampleSize { get; set; }
}