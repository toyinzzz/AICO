using AICO.Domain.Entities;

namespace AICO.Application.Interfaces.Commands
{
    public interface ICampaignCommandHandler
    {
        Task<Campaign> CreateCampaignAsync(CreateCampaignCommand command);
        Task<Campaign> UpdateCampaignAsync(UpdateCampaignCommand command);
        Task DeleteCampaignAsync(Guid campaignId);
        Task<Campaign> StartCampaignAsync(Guid campaignId);
        Task<Campaign> StopCampaignAsync(Guid campaignId);
        Task<Campaign> CompleteCampaignAsync(Guid campaignId);
    }

    public record CreateCampaignCommand(
        Guid WebsiteId,
        string Name,
        string Description,
        DateTime StartDate,
        DateTime? EndDate,
        decimal Budget,
        string TargetUrl
    );

    public record UpdateCampaignCommand(
        Guid Id,
        string Name,
        string Description,
        DateTime StartDate,
        DateTime? EndDate,
        decimal Budget,
        string TargetUrl
    );
}