using System;

namespace AICO.Application.DTOs
{
    public class VariantFilter
    {
        public Guid? AbTestId { get; set; }
        public bool? IsControl { get; set; }
        public string? NameContains { get; set; }
        public int? MinTrafficAllocation { get; set; }
        public int? MaxTrafficAllocation { get; set; }
    }
}