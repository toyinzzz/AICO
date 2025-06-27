using AICO.Application.Interfaces.Commands;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Commands
{
    public class CampaignCommandHandler : ICampaignCommandHandler
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignCommandHandler(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository ?? throw new ArgumentNullException(nameof(campaignRepository));
        }

        public Task<Campaign> HandleAsync(CreateCampaignCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<Campaign> HandleAsync(UpdateCampaignCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task HandleAsync(DeleteCampaignCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<Campaign> HandleAsync(StartCampaignCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<Campaign> HandleAsync(StopCampaignCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            throw new NotImplementedException();
        }

        public Task<Campaign> HandleAsync(CompleteCampaignCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            // Add validation as needed
            throw new NotImplementedException();
        }
    }
}