using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Events;

namespace AICO.Domain.Events
{
    public class AbTestStatusChangedEvent : IDomainEvent
    {
        public Guid AbTestId { get; }
        public AbTestStatus OldStatus { get; }
        public AbTestStatus NewStatus { get; }
        public DateTime OccurredOn { get; }

        public AbTestStatusChangedEvent(Guid abTestId, AbTestStatus oldStatus, AbTestStatus newStatus)
        {
            AbTestId = abTestId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            OccurredOn = DateTime.UtcNow;
        }
    }
}