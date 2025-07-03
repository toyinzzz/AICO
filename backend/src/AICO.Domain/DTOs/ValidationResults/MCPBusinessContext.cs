namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Business context for MCP validation
/// </summary>
public class MCPBusinessContext
{
    public string Industry { get; set; } = string.Empty;
    public string BusinessModel { get; set; } = string.Empty;
    public decimal ExpectedMCPRange { get; set; }
    public decimal MaxAcceptableMCP { get; set; } = 1000m; // 1000% max
    public decimal MinMeaningfulMCP { get; set; } = 1m; // 1% minimum
    public bool IsHighRiskBusiness { get; set; }
    public List<string> BusinessConstraints { get; set; } = new();
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal CalculatedMCP { get; set; }
}