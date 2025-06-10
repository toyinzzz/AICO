using System;
using System.Collections.Generic;
using AICO.Domain.Events;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Base class for all entities
    /// </summary>
    public abstract class BaseEntity
    {
        private readonly List<DomainEvent> _domainEvents = new();
        
        /// <summary>
        /// Entity identifier
        /// </summary>
        public Guid Id { get; protected set; } = Guid.NewGuid();
        
        /// <summary>
        /// When the entity was created
        /// </summary>
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        
        /// <summary>
        /// When the entity was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; protected set; }
        
        /// <summary>
        /// Row version for optimistic concurrency
        /// </summary>
        public byte[] RowVersion { get; protected set; }
        
        /// <summary>
        /// Whether the entity is deleted (soft delete)
        /// </summary>
        public bool IsDeleted { get; protected set; }
        
        /// <summary>
        /// Domain events raised by this entity
        /// </summary>
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        /// <summary>
        /// Adds a domain event
        /// </summary>
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        
        /// <summary>
        /// Clears all domain events
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
        
        /// <summary>
        /// Marks the entity as updated
        /// </summary>
        protected void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Marks the entity as deleted (soft delete)
        /// </summary>
        public virtual void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                MarkAsUpdated();
            }
        }
    }
}