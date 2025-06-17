using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;
// using AICO.Domain.Interfaces.common; // Removed incorrect using
using System;
using AICO.Domain.Interfaces; // This should resolve IAuditableEntity

namespace AICO.Domain.Services
{
    /// <summary>
    /// Service for handling audit-related operations on entities
    /// </summary>
    public class AuditService : IAuditService
    {
        /// <summary>
        /// Updates the modification date of an entity as per IAuditService.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">User ID who performed the modification</param>
        public void UpdateModificationDate(IAuditableEntity entity, string? userId)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.UpdateModificationDate(userId);
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {entity.GetType().Name} is not a BaseEntity and cannot be updated by this method.");
            }
        }

        /// <summary>
        /// Sets the creation audit information for an entity as per IAuditService.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="userId">User ID who created the entity</param>
        public void SetCreationAudit(IAuditableEntity entity, string? userId)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.SetAuditInfo(userId, null); // Sets CreatedBy, ModifiedBy will be null initially
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {entity.GetType().Name} is not a BaseEntity and cannot have audit info set by this method.");
            }
        }

        /// <summary>
        /// Sets the creation audit information for an AnalysisResult entity.
        /// </summary>
        /// <param name="analysis">The AnalysisResult entity to update</param>
        public void SetCreationAudit(AnalysisResult analysis)
        {
            if (analysis == null)
                throw new ArgumentNullException(nameof(analysis));
            
            // AnalysisResult is expected to be an IAuditableEntity (likely via BaseEntity)
            SetCreationAudit((IAuditableEntity)analysis, "System"); // Default to "System" user
        }

        /// <summary>
        /// Updates the modification date of an entity (overload from IAuditService).
        /// </summary>
        /// <param name="auditableEntity">The entity to update</param>
        public void UpdateModificationDate(IAuditableEntity auditableEntity)
        {
            if (auditableEntity == null)
                throw new ArgumentNullException(nameof(auditableEntity));

            if (auditableEntity is BaseEntity baseEntity)
            {
                // Assuming "System" or a default user if no specific user ID is provided via this overload.
                baseEntity.UpdateModificationDate("System"); 
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {auditableEntity.GetType().Name} is not a BaseEntity and cannot be updated by this method.");
            }
        }

        /// <summary>
        /// Sets the creation audit information for a Recommendation entity.
        /// </summary>
        /// <param name="recommendation">The Recommendation entity to update</param>
        public void SetCreationAudit(Recommendation recommendation)
        {
            if (recommendation == null)
                throw new ArgumentNullException(nameof(recommendation));

            // Recommendation is expected to be an IAuditableEntity (likely via BaseEntity)
            SetCreationAudit((IAuditableEntity)recommendation, "System"); // Default to "System" user
        }

        // Removing the following duplicate methods:
        // public void UpdateModificationDate(IAuditableEntity entity, string? userId)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public void SetCreationAudit(IAuditableEntity entity, string? userId)
        // {
        //     throw new NotImplementedException();
        // }
        //
        // public void UpdateModificationDate(IAuditableEntity auditableEntity)
        // {
        //     throw new NotImplementedException();
        // }
    }
}