using System;

namespace AICO.Domain.Interfaces
{
    /// <summary>
    /// Interface for entities that require auditing
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        DateTime CreatedAt { get; }
        
        /// <summary>
        /// Date and time when the entity was last modified
        /// </summary>
        DateTime? ModifiedAt { get; }
        
        /// <summary>
        /// User ID who created the entity
        /// </summary>
        string CreatedBy { get; }
        
        /// <summary>
        /// User ID who last modified the entity
        /// </summary>
        string ModifiedBy { get; }
    }
} 