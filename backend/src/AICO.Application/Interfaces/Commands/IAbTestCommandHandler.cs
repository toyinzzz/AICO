using AICO.Domain.Entities;

namespace AICO.Application.Interfaces.Commands
{
    // Command Records
    public record CreateAbTestCommand(
        Guid CampaignId,
        string Name,
        string Description,
        string TestType,
        int V,
        string TargetSelector,
        string OriginalContent,
        string PrimaryMetric,
        int TrafficSplit,
        DateTime StartDate,
        DateTime? EndDate
    ) : ICommand<AbTest>;

    public record UpdateAbTestCommand(
        Guid Id,
        string Name,
        string Description,
        string TestType,
        string TargetSelector,
        string OriginalContent,
        string PrimaryMetric,
        int TrafficSplit,
        DateTime StartDate,
        DateTime? EndDate
    ) : ICommand<AbTest>;

    public record DeleteAbTestCommand(Guid AbTestId) : ICommand;

    public record StartAbTestCommand(Guid AbTestId) : ICommand<AbTest>;

    public record StopAbTestCommand(Guid AbTestId) : ICommand<AbTest>;

    public record CompleteAbTestCommand(Guid AbTestId) : ICommand<AbTest>;

    // Command Handler Interface
    public interface IAbTestCommandHandler :
        ICommandHandler<CreateAbTestCommand, AbTest>,
        ICommandHandler<UpdateAbTestCommand, AbTest>,
        ICommandHandler<DeleteAbTestCommand>,
        ICommandHandler<StartAbTestCommand, AbTest>,
        ICommandHandler<StopAbTestCommand, AbTest>,
        ICommandHandler<CompleteAbTestCommand, AbTest>
    {
        Task DeleteAbTestAsync(Guid id);
        Task CompleteAbTestAsync(Guid id);
        Task<AbTest> CreateAbTestAsync(CreateAbTestCommand command); // Fixed generic type
        Task StartAbTestAsync(Guid id);
        Task StopAbTestAsync(Guid id);
        Task UpdateAbTestAsync(UpdateAbTestCommand command);
    }
}