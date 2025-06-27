using System.ComponentModel.DataAnnotations;
using AICO.Domain.Events;
using AICO.Domain.Interfaces;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Base class for all entities
    /// </summary>
    public abstract class BaseEntity : IAuditableEntity

    {
        private readonly List<DomainEvent> _domainEvents = new();

        /// <summary>
        /// Entity identifier
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// When the entity was created
        /// </summary>
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        /// <summary>
        /// When the entity was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; protected set; }

        /// <summary>
        /// Date and time when the entity was last modified (alias for UpdatedAt)
        /// </summary>
        public DateTime? ModifiedAt => UpdatedAt;

        /// <summary>
        /// User ID who created the entity
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// User ID who last modified the entity
        /// </summary>
        public string ModifiedBy { get; set; } = string.Empty;

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
        /// Sets audit information for the entity
        /// </summary>
        /// <param name="createdBy">User who created the entity</param>
        /// <param name="modifiedBy">User who modified the entity</param>
        public void SetAuditInfo(string createdBy, string? modifiedBy = null)
        {
            if (!string.IsNullOrWhiteSpace(createdBy))
            {
                CreatedBy = createdBy;
            }
            
            if (!string.IsNullOrWhiteSpace(modifiedBy))
            {
                ModifiedBy = modifiedBy;
                MarkAsUpdated();
            }
            // Ensure ModifiedBy is not null if it wasn't set and CreatedBy was
            else if (!string.IsNullOrWhiteSpace(createdBy) && string.IsNullOrEmpty(ModifiedBy))
            {
                ModifiedBy = string.Empty; // Or some other default non-null value if appropriate
            }
        }

        /// <summary>
        /// Updates the modification date and user
        /// </summary>
        /// <param name="modifiedBy">User who modified the entity</param>
        public void UpdateModificationDate(string? modifiedBy = null)
        {
            ModifiedBy = modifiedBy ?? "System";
            MarkAsUpdated();
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