using AICO.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Commands
{
    // Command Records
    public record CreateVariantCommand(
        Guid AbTestId,
        Guid CampaignId,
        string Name,
        string Content,
        bool IsControl,
        int TrafficPercentage
    ) : ICommand<Variant>;

    public record UpdateVariantCommand(
        Guid Id,
        string Name,
        string Content,
        bool IsControl,
        int TrafficPercentage
    ) : ICommand<Variant>;

    public record DeleteVariantCommand(Guid VariantId) : ICommand;

    public record GenerateAIVariantCommand(
        Guid AbTestId,
        Guid CampaignId,
        string OriginalContent,
        string OptimizationGoal,
        string TargetAudience
    ) : ICommand<Variant>;

    // Command Handler Interface
    public interface IVariantCommandHandler :
        ICommandHandler<CreateVariantCommand, Variant>,
        ICommandHandler<UpdateVariantCommand, Variant>,
        ICommandHandler<DeleteVariantCommand>,
        ICommandHandler<GenerateAIVariantCommand, Variant>
    {
    }
}