using AICO.Application.Interfaces.Commands;
using AICO.Domain.Entities;
using System.Collections.Generic;

namespace AICO.Application.Commands
{
    public class GenerateVariantCommand : ICommand<List<Variant>>
    {
        public required string OriginalPageUrl { get; set; }
        public required string OptimizationGoal { get; set; } // e.g., 'Increase-CTR', 'Improve-Conversion'
        public required string TargetAudience { get; set; } // e.g., 'New-Visitors', 'Returning-Customers'
        public required string ContentType { get; set; } // e.g., 'Headline', 'CTA-Button'
        public required string[] Keywords { get; set; } // e.g., ['AI', 'Optimization']
    }
}