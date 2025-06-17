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
    }
}