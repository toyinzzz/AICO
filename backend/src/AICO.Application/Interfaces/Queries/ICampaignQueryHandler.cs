using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    // Query Records
    public record GetCampaignByIdQuery(Guid Id) : IQuery<Campaign?>;
    public record GetCampaignsByWebsiteIdQuery(Guid WebsiteId) : IQuery<IEnumerable<Campaign>>;
    public record GetActiveCampaignsQuery() : IQuery<IEnumerable<Campaign>>;
    public record GetCampaignsByStatusQuery(string Status) : IQuery<IEnumerable<Campaign>>;
    public record GetCampaignsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<Campaign>>;

    // Query Handler Interface
    public interface ICampaignQueryHandler :
        IQueryHandler<GetCampaignByIdQuery, Campaign?>,
        IQueryHandler<GetCampaignsByWebsiteIdQuery, IEnumerable<Campaign>>,
        IQueryHandler<GetActiveCampaignsQuery, IEnumerable<Campaign>>,
        IQueryHandler<GetCampaignsByStatusQuery, IEnumerable<Campaign>>,
        IQueryHandler<GetCampaignsByDateRangeQuery, IEnumerable<Campaign>>
    {
    }
}