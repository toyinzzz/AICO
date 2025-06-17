using AICO.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Commands
{
    // Command Records
    public record CreateCampaignCommand(
        Guid WebsiteId,
        string Name,
        string Description,
        DateTime StartDate,
        DateTime? EndDate,
        decimal Budget,
        string TargetUrl
    ) : ICommand<Campaign>;

    public record UpdateCampaignCommand(
        Guid Id,
        string Name,
        string Description,
        DateTime StartDate,
        DateTime? EndDate,
        decimal Budget,
        string TargetUrl
    ) : ICommand<Campaign>;

    public record DeleteCampaignCommand(Guid CampaignId) : ICommand;

    public record StartCampaignCommand(Guid CampaignId) : ICommand<Campaign>;

    public record StopCampaignCommand(Guid CampaignId) : ICommand<Campaign>;

    public record CompleteCampaignCommand(Guid CampaignId) : ICommand<Campaign>;

    // Command Handler Interface
    public interface ICampaignCommandHandler :
        ICommandHandler<CreateCampaignCommand, Campaign>,
        ICommandHandler<UpdateCampaignCommand, Campaign>,
        ICommandHandler<DeleteCampaignCommand>,
        ICommandHandler<StartCampaignCommand, Campaign>,
        ICommandHandler<StopCampaignCommand, Campaign>,
        ICommandHandler<CompleteCampaignCommand, Campaign>
    {
    }
}