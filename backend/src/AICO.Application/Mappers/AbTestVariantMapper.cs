using AICO.Shared.Interfaces;
using AICO.Domain.Entities;

namespace AICO.Application.Mappers
{
    public class AbTestVariantMapper : IMapper<AbTestVariant, Variant>
    {
        public Variant Map(AbTestVariant source)
        {
            if (source == null) return null;

            var variant = new Variant(
                source.Name,
                source.AbTestId,
                source.Content,
                source.TrafficSplitPercentage,
                source.IsControl,
                null, // aiPrompt is not in AbTestVariant
                source.AiConfidenceScore
            );

            // Manually map properties not in the constructor
            typeof(BaseEntity).GetProperty("Id").SetValue(variant, source.Id);
            var viewsProperty = typeof(Variant).GetProperty("Views");
            if (viewsProperty != null)
            {
                viewsProperty.SetValue(variant, (int)source.Views);
            }
            typeof(Variant).GetProperty("Conversions").SetValue(variant, (int)source.Conversions);

            return variant;
        }

        public AbTestVariant Map(Variant destination)
        {
            if (destination == null) return null;

            var abTestVariant = new AbTestVariant(
                destination.AbTestId,
                destination.Name,
                destination.Content,
                destination.TrafficAllocation,
                destination.IsControl.ToString()
            );

            // Manually map properties not in the constructor
            typeof(BaseEntity).GetProperty("Id").SetValue(abTestVariant, destination.Id);
            typeof(AbTestVariant).GetProperty("Views").SetValue(abTestVariant, destination.Views);
            typeof(AbTestVariant).GetProperty("Conversions").SetValue(abTestVariant, destination.Conversions);

            return abTestVariant;
        }
    }
}