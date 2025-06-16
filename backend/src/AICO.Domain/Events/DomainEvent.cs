namespace AICO.Domain.Events
{
    /// <summary>
    /// Base class for all domain events
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// Unique identifier for the event
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// When the event occurred
        /// </summary>
        public DateTime OccurredOn { get; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
}