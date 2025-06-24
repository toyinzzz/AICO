using AICO.Shared.Interfaces;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Application.Mappers
{
    public class RevenueMapper : IMapper<Revenue, RevenueDto>
    {
        public RevenueDto Map(Revenue source)
        {
            if (source == null) return null;
            return new RevenueDto
            {
                Id = source.Id,
                WebsiteId = source.WebsiteId,
                UserId = source.UserId,
                ConversionId = source.ConversionId,
                CampaignId = source.CampaignId ?? Guid.Empty,
                Amount = source.Amount,
                Currency = source.Currency,
                Source = source.Source,
                TransactionId = source.TransactionId,
                Metadata = source.Metadata,
                RevenueDate = source.RevenueDate,
                SessionId = source.SessionId,
                VariantId = (Guid)source.VariantId,
                IsControl = source.IsControl
            };
        }

        public Revenue Map(RevenueDto destination)
        {
            if (destination == null) return null;
            var revenue = new Revenue(
                destination.WebsiteId,
                destination.UserId,
                destination.ConversionId,
                destination.CampaignId,
                destination.Amount,
                destination.Currency,
                destination.Source,
                destination.TransactionId,
                destination.Metadata,
                destination.RevenueDate,
                destination.SessionId,
                destination.VariantId,
                destination.IsControl
            );
            return revenue;
        }
    }
}