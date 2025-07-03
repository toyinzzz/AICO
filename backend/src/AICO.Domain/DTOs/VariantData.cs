namespace AICO.Domain.DTOs;

public class VariantData
{
    public Guid VariantId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int Conversions { get; set; }
    public int Visitors { get; set; }
    public decimal Cost { get; set; }
    public bool IsControl { get; set; }
    public decimal ConversionRate => Visitors > 0 ? (decimal)Conversions / Visitors : 0;
    public decimal RevenuePerConversion => Conversions > 0 ? Revenue / Conversions : 0;
    public decimal RevenuePerVisitor => Visitors > 0 ? Revenue / Visitors : 0;
}