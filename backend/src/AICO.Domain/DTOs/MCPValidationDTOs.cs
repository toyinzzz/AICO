namespace AICO.Domain.DTOs;

/// <summary>
/// Basic MCP input validation result
/// </summary>
public class MCPValidationDTO
{
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
    public List<string> ValidationWarnings { get; set; } = new();
    public string ValidationStatus { get; set; } = string.Empty;
}