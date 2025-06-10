// BaseEntity.cs

using System;
using AICO.Domain.Interfaces;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Base class for all domain entities with common properties
    /// </summary>
    public abstract class BaseEntity : IAuditableEntity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Date and time when the entity was last modified
        /// </summary>
        public DateTime? ModifiedAt { get; private set; }
        
        /// <summary>
        /// User ID who created the entity
        /// </summary>
        public string CreatedBy { get; private set; }
        
        /// <summary>
        /// User ID who last modified the entity
        /// </summary>
        public string ModifiedBy { get; private set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }

        // Internal methods for audit service
        internal void SetAuditInfo(string createdBy, string modifiedBy = null)
        {
            CreatedBy = createdBy;
            if (modifiedBy != null)
            {
                ModifiedBy = modifiedBy;
                ModifiedAt = DateTime.UtcNow;
            }
        }

        // Internal method to update the modification date
        internal void UpdateModificationDate()
        {
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
 