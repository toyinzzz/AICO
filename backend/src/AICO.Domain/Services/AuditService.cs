using AICO.Domain.Entities;
using AICO.Domain.Interfaces;

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for handling audit-related operations on entities
    /// </summary>
    public class AuditService : IAuditService
    {
        /// <summary>
        /// Updates the modification date of an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">Optional user ID who performed the modification</param>
        public void UpdateModificationDate(IAuditableEntity entity, string userId = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.UpdateModificationDate(userId);
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {entity.GetType().Name} is not a BaseEntity and cannot be updated.");
            }
        }

        /// <summary>
        /// Sets the creation audit information for an entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">Optional user ID who created the entity</param>
        public void SetCreationAudit(IAuditableEntity entity, string userId = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.SetAuditInfo(userId ?? "System", null);
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {entity.GetType().Name} is not a BaseEntity and cannot be updated.");
            }
        }

        /// <summary>
        /// Sets the creation audit information for an AnalysisResult entity
        /// </summary>
        /// <param name="analysis">The AnalysisResult entity to update</param>
        public void SetCreationAudit(AnalysisResult analysis)
        {
            if (analysis == null)
                throw new ArgumentNullException(nameof(analysis));

            // AnalysisResult inherits from BaseEntity, so we can set audit info
            SetCreationAudit((IAuditableEntity)analysis, "System");
        }
    }
}