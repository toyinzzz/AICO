using AICO.Application.Interfaces.Queries;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Queries
{
    public class AbTestQueryHandler : IAbTestQueryHandler
    {
        private readonly IAbTestRepository _abTestRepository;

        public AbTestQueryHandler(IAbTestRepository abTestRepository)
        {
            _abTestRepository = abTestRepository ?? throw new ArgumentNullException(nameof(abTestRepository));
        }

        public Task<AbTest> GetAbTestByIdAsync(Guid campaignId)
        {
            throw new NotImplementedException();
        }

        public async Task<AbTest?> HandleAsync(GetAbTestByIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (query.Id == Guid.Empty) throw new ArgumentException("A/B test ID cannot be empty.", nameof(query.Id));
            
            return await _abTestRepository.GetByIdAsync(query.Id);
        }

        public async Task<IEnumerable<AbTest>> HandleAsync(GetAbTestsByCampaignIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (query.CampaignId == Guid.Empty) throw new ArgumentException("Campaign ID cannot be empty.", nameof(query.CampaignId));
            return await _abTestRepository.GetByCampaignIdAsync(query.CampaignId);
        }

        public async Task<IEnumerable<AbTest>> HandleAsync(GetActiveAbTestsQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            return await _abTestRepository.GetActiveAsync();
        }

        public async Task<IEnumerable<AbTest>> HandleAsync(GetRunningAbTestsQuery query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return await _abTestRepository.GetByStatusAsync(AbTestStatus.Running);
        }

        public async Task<IEnumerable<AbTest>> HandleAsync(GetCompletedAbTestsQuery query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return await _abTestRepository.GetByStatusAsync(AbTestStatus.Completed);
        }
    }
}