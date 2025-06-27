using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for User entity operations
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets a user by email
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Gets a user by username
        /// </summary>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Checks if an email is already in use
        /// </summary>
        Task<bool> IsEmailInUseAsync(string email);

        /// <summary>
        /// Checks if a username is already in use
        /// </summary>
        Task<bool> IsUsernameInUseAsync(string username);

        /// <summary>
        /// Gets all users for a specific website (optional, falls relevant)
        /// </summary>
        Task<IEnumerable<User>> GetByWebsiteIdAsync(Guid websiteId);

        /// <summary>
        /// Updates the user's last login date
        /// </summary>
        Task UpdateLastLoginDateAsync(Guid userId);

        /// <summary>
        /// Sets the user's email as verified
        /// </summary>
        Task SetEmailVerifiedAsync(Guid userId);

        /// <summary>
        /// Gets a user by verification token (optional, falls Token gespeichert wird)
        /// </summary>
        Task<User?> GetByVerificationTokenAsync(string token);

        /// <summary>
        /// Sets a verification token for the user (optional, falls Token gespeichert wird)
        /// </summary>
        Task SetVerificationTokenAsync(Guid userId, string token);

        /// <summary>
        /// Removes a verification token after successful verification (optional)
        /// </summary>
        Task RemoveVerificationTokenAsync(Guid userId);
        Task<UserAssignedVariantDto?> GetUserAbTestVariantAsync(string userId, Guid abTestId);
        Task AssignUserToAbTestVariantAsync(string userId, Guid abTestId, Guid variantId);

    }
}