using System;
using System.Collections.Generic;

namespace AICO.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string Category { get; private set; } // e.g., "UserManagement", "Billing", "Content"
        public virtual ICollection<RolePermission> RolePermissions { get; private set; }

        private Permission() 
        {
            Name = string.Empty; // Initialize Name
            Category = string.Empty; // Initialize Category
            RolePermissions = new HashSet<RolePermission>();
        } // Required for EF Core

        public static Permission Create(string name, string category, string? description = null)
        {
            var permission = new Permission
            {
                Name = name,
                Category = category,
                Description = description
            };
            return permission;
        }

        public void Update(string name, string category, string? description)
        {
            Name = name;
            Category = category;
            Description = description;
        }
    }
}