using AICO.Shared.Interfaces;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Application.Mappers
{
    public class VariantMapper : IMapper<Variant, VariantDto>
    {
        public VariantDto Map(Variant source)
        {
            if (source == null) return null;
            return new VariantDto
            {
                Id = source.Id,
                AbTestId = source.AbTestId,
                Name = source.Name,
                Content = source.Content,
                TrafficAllocation = source.TrafficAllocation,
                IsControl = source.IsControl,
                Views = source.Views,
                Conversions = source.Conversions,
                ConversionRate = (double)source.GetConversionRate()
            };
        }

        public Variant Map(VariantDto destination)
        {
            if (destination == null) return null;
            // Note: Mapping from DTO to Entity might require more complex logic,
            // especially if the entity has state or behavior not present in the DTO.
            // For now, a simple mapping is provided.
            var variant = new Variant(destination.Name, destination.AbTestId, destination.Content, destination.TrafficAllocation, destination.IsControl);

            // Since Id, Views and Conversions are not part of the constructor, we can set them separately if needed.
            // This assumes we have a way to update them, for example, if they are not private set.
            // If they have private setters, we would need methods in the entity to update them.

            return variant;
        }
    }
}