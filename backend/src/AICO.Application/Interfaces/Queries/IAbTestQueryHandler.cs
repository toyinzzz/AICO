using AICO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Queries
{
    // Query Records
    public record GetAbTestByIdQuery(Guid Id) : IQuery<AbTest?>;
    public record GetAbTestsByCampaignIdQuery(Guid CampaignId) : IQuery<IEnumerable<AbTest>>;
    public record GetActiveAbTestsQuery() : IQuery<IEnumerable<AbTest>>;
    public record GetRunningAbTestsQuery() : IQuery<IEnumerable<AbTest>>;
    public record GetCompletedAbTestsQuery() : IQuery<IEnumerable<AbTest>>;

    // Query Handler Interface
    public interface IAbTestQueryHandler :
        IQueryHandler<GetAbTestByIdQuery, AbTest?>,
        IQueryHandler<GetAbTestsByCampaignIdQuery, IEnumerable<AbTest>>,
        IQueryHandler<GetActiveAbTestsQuery, IEnumerable<AbTest>>,
        IQueryHandler<GetRunningAbTestsQuery, IEnumerable<AbTest>>,
        IQueryHandler<GetCompletedAbTestsQuery, IEnumerable<AbTest>>
    {
    }
}