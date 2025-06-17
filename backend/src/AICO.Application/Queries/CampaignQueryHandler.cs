using AICO.Application.Interfaces.Queries;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AICO.Application.Queries
{
    public class CampaignQueryHandler : ICampaignQueryHandler
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignQueryHandler(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
        }

        public Task<Campaign?> HandleAsync(GetCampaignByIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> HandleAsync(GetCampaignsByWebsiteIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> HandleAsync(GetActiveCampaignsQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> HandleAsync(GetCampaignsByStatusQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> HandleAsync(GetCampaignsByDateRangeQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            // Add validation as needed
            throw new NotImplementedException();
        }
    }
}