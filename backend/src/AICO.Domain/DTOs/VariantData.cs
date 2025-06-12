namespace AICO.Domain.DTOs
{
    public class VariantData
    {
        public Guid VariantId { get; set; }
        public decimal Revenue { get; set; }
        public int Conversions { get; set; }
        public bool IsControl { get; set; }
    }
}