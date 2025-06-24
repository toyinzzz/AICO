namespace AICO.Domain.Interfaces.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}