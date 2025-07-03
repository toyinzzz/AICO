namespace AICO.Domain.DTOs.ValidationResults;

/// <summary>
/// Basic MCP input validation result
/// </summary>
public class MCPInputValidationResult
{
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ErrorMessages { get; set; } = new();
    public List<string> ValidationMessages { get; set; } = new();
    public decimal CalculatedMCP { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal RevenuePerVisitor { get; set; }
    public decimal CostPerConversion { get; set; }
}