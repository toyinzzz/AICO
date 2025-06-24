using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    // Query Records
    public record GetVariantByIdQuery(Guid Id) : IQuery<Variant?>;
    public record GetVariantsByAbTestIdQuery(Guid AbTestId) : IQuery<IEnumerable<Variant>>;
    public record GetVariantsByCampaignIdQuery(Guid CampaignId) : IQuery<IEnumerable<Variant>>;
    public record GetControlVariantQuery(Guid AbTestId) : IQuery<Variant?>;
    public record GetTestVariantsQuery(Guid AbTestId) : IQuery<IEnumerable<Variant>>;

    // Query Handler Interface
    public interface IVariantQueryHandler :
        IQueryHandler<GetVariantByIdQuery, Variant?>,
        IQueryHandler<GetVariantsByAbTestIdQuery, IEnumerable<Variant>>,
        IQueryHandler<GetVariantsByCampaignIdQuery, IEnumerable<Variant>>,
        IQueryHandler<GetControlVariantQuery, Variant?>,
        IQueryHandler<GetTestVariantsQuery, IEnumerable<Variant>>
    {
    }
}