using AICO.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Commands
{
    public interface IVariantCommandHandler
    {
        Task<Variant> CreateVariantAsync(CreateVariantCommand command);
        Task<Variant> UpdateVariantAsync(UpdateVariantCommand command);
        Task DeleteVariantAsync(Guid variantId);
        Task<Variant> GenerateAIVariantAsync(GenerateAIVariantCommand command);
    }

    public record CreateVariantCommand(
        Guid AbTestId,
        Guid CampaignId,
        string Name,
        string Content,
        bool IsControl,
        int TrafficPercentage
    );

    public record UpdateVariantCommand(
        Guid Id,
        string Name,
        string Content,
        bool IsControl,
        int TrafficPercentage
    );

    public record GenerateAIVariantCommand(
        Guid AbTestId,
        Guid CampaignId,
        string OriginalContent,
        string OptimizationGoal,
        string TargetAudience
    );
}