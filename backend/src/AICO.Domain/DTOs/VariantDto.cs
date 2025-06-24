using System;

namespace AICO.Domain.DTOs
{
    public class VariantDto
    {
        public Guid Id { get; set; }
        public Guid AbTestId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public decimal TrafficAllocation { get; set; }
        public bool IsControl { get; set; }
        public int Views { get; set; }
        public int Conversions { get; set; }
        public double ConversionRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        
    }
}