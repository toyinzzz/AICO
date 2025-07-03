namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Business rule validation result
/// </summary>
public class BusinessRuleValidationResult
{
    public bool IsValid { get; set; }
    public bool ExceedsThresholds { get; set; }
    public bool IsRealistic { get; set; }
    public decimal MCPValue { get; set; }
    public decimal MinimumThreshold { get; set; }
    public decimal MaximumThreshold { get; set; }
    public List<string> RuleViolations { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ValidationMessages { get; set; } = new();
    public List<string> AppliedRules { get; set; } = new();
    public string Industry { get; set; } = string.Empty;
    public double ComplianceScore { get; set; }
    public string RiskLevel { get; set; } = "Low"; // Low, Medium, High
}