// IAuditService.cs

using AICO.Domain.Entities;
// using AICO.Domain.Interfaces.common; // Removed incorrect using
using AICO.Domain.Interfaces; // This should resolve IAuditableEntity

namespace AICO.Domain.Interfaces.Services // Changed namespace
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
        void UpdateModificationDate(IAuditableEntity entity, string? userId );

        /// <summary>
        /// Sets the creation audit information for an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">Optional user ID who created the entity</param>
        void SetCreationAudit(IAuditableEntity entity, string? userId );
        void SetCreationAudit(AnalysisResult analysis);
        void UpdateModificationDate(IAuditableEntity auditableEntity);
        void SetCreationAudit(Recommendation recommendation);
    }
}