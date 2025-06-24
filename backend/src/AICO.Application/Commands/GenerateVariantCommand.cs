using AICO.Application.Interfaces.Commands;
using AICO.Domain.Entities;
using System.Collections.Generic;

namespace AICO.Application.Commands
{
    public class GenerateVariantCommand : ICommand<List<Variant>>
    {
        public string OriginalPageUrl { get; set; }
        public string OptimizationGoal { get; set; } // e.g., 'Increase-CTR', 'Improve-Conversion'
        public string TargetAudience { get; set; } // e.g., 'New-Visitors', 'Returning-Customers'
        public string ContentType { get; set; } // e.g., 'Headline', 'CTA-Button'
        public string[] Keywords { get; set; } // e.g., ['AI', 'Optimization']
    }
}