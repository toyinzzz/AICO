using System;

namespace AICO.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        public Guid RoleId { get; private set; }
        public virtual Role Role { get; private set; }

        private UserRole() 
        {
            User = null!; // Initialize User
            Role = null!; // Initialize Role
        } // Required for EF Core

        public static UserRole Create(Guid userId, Guid roleId)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };
            return userRole;
        }
    }
}