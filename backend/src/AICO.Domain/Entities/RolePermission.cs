using System;

namespace AICO.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; private set; }
        public virtual Role Role { get; private set; }

        public Guid PermissionId { get; private set; }
        public virtual Permission Permission { get; private set; }

        private RolePermission() 
        {
            Role = null!; // Initialize Role
            Permission = null!; // Initialize Permission
        } // Required for EF Core

        public static RolePermission Create(Guid roleId, Guid permissionId)
        {
            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };
            return rolePermission;
        }
    }
}