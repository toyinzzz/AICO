using AICO.Domain.Entities;

namespace AICO.Application.Interfaces.Commands
{
    public interface IAbTestCommandHandler
    {
        Task<AbTest> CreateAbTestAsync(CreateAbTestCommand command);
        Task<AbTest> UpdateAbTestAsync(UpdateAbTestCommand command);
        Task DeleteAbTestAsync(Guid abTestId);
        Task<AbTest> StartAbTestAsync(Guid abTestId);
        Task<AbTest> StopAbTestAsync(Guid abTestId);
        Task<AbTest> CompleteAbTestAsync(Guid abTestId);
    }

    public record CreateAbTestCommand(
        Guid CampaignId,
        string Name,
        string Description,
        string TestType,
        int TrafficSplit,
        string SuccessMetric,
        DateTime StartDate,
        DateTime? EndDate
    );

    public record UpdateAbTestCommand(
        Guid Id,
        string Name,
        string Description,
        string TestType,
        int TrafficSplit,
        string SuccessMetric,
        DateTime StartDate,
        DateTime? EndDate
    );
}