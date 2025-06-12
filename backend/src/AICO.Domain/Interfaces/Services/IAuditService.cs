// IAuditService.cs

using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces
{
    /// <summary>
    /// Service for handling audit-related operations on entities
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Updates the modification date of an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">Optional user ID who performed the modification</param>
        void UpdateModificationDate(IAuditableEntity entity, string userId = null);

        /// <summary>
        /// Sets the creation audit information for an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">Optional user ID who created the entity</param>
        void SetCreationAudit(IAuditableEntity entity, string userId = null);
        void SetCreationAudit(AnalysisResult analysis);
    }
}