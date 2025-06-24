using System;
using System.Collections.Generic;

namespace AICO.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public virtual ICollection<UserRole> UserRoles { get; private set; }
        public virtual ICollection<RolePermission> RolePermissions { get; private set; }

        private Role() 
        {
            Name = string.Empty; // Initialize Name
            UserRoles = new HashSet<UserRole>();
            RolePermissions = new HashSet<RolePermission>();
        } // Required for EF Core

        public static Role Create(string name, string? description = null)
        {
            var role = new Role
            {
                Name = name,
                Description = description
            };
            return role;
        }

        public void Update(string name, string? description)
        {
            Name = name;
            Description = description;
        }
    }
}