using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    // Query Records
    public record GetConversionEventsByCampaignIdQuery(Guid CampaignId, DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<ConversionEvent>>;
    public record GetConversionEventsByAbTestIdQuery(Guid AbTestId, DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<ConversionEvent>>;
    public record GetConversionEventsByVariantIdQuery(Guid VariantId, DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<ConversionEvent>>;
    public record GetConversionRateQuery(Guid EntityId, string EntityType, DateTime StartDate, DateTime EndDate) : IQuery<decimal>; // EntityType could be "Campaign", "AbTest", "Variant"

    // Query Handler Interface
    public interface IConversionEventQueryHandler :
        IQueryHandler<GetConversionEventsByCampaignIdQuery, IEnumerable<ConversionEvent>>,
        IQueryHandler<GetConversionEventsByAbTestIdQuery, IEnumerable<ConversionEvent>>,
        IQueryHandler<GetConversionEventsByVariantIdQuery, IEnumerable<ConversionEvent>>,
        IQueryHandler<GetConversionRateQuery, decimal>
    {
    }
}