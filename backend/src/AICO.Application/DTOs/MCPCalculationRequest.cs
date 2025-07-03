using System.ComponentModel.DataAnnotations;

namespace AICO.Application.DTOs;

/// <summary>
/// Request model for MCP calculation
/// </summary>
public class MCPCalculationRequest
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Control revenue must be non-negative")]
    public decimal ControlRevenue { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Control conversions must be greater than 0")]
    public int ControlConversions { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Variant revenue must be non-negative")]
    public decimal VariantRevenue { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Variant conversions must be greater than 0")]
    public int VariantConversions { get; set; }

    [MaxLength(3)]
    public string? Currency { get; set; } = "USD";
}