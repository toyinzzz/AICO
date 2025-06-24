using System;

namespace AICO.Domain.Entities
{
    public class ConversionEvent : BaseEntity
    {
        public Guid SessionId { get; set; }
        public Guid? VariantId { get; set; } // Nullable if conversion can happen without a specific variant
        public Guid? CampaignId { get; set; }
        public Guid? AbTestId { get; set; }
        public string EventName { get; set; } = string.Empty; // e.g., "SignUp", "Purchase", "PageView"
        public DateTime EventTimestamp { get; set; }
        public decimal? Value { get; set; } // Optional monetary value of the conversion
        public string? Currency { get; set; } // e.g., "USD", "EUR"
        public string? Source { get; set; } // e.g., "Direct", "Organic Search", "PPC"
        public string? Medium { get; set; } // e.g., "Website", "Email", "Social"
        public string? Notes { get; set; } // Any additional notes or metadata

        // Navigation properties (optional, depending on your EF Core setup)
        // public virtual Session Session { get; set; }
        // public virtual Variant Variant { get; set; }
        // public virtual Campaign Campaign { get; set; }
        // public virtual AbTest AbTest { get; set; }

        public ConversionEvent()
        {
            EventTimestamp = DateTime.UtcNow;
        }
    }
}