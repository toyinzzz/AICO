using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service interface for User-related business logic
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Authenticates a user
        /// </summary>
        Task<User> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Registers a new user
        /// </summary>
        Task<User> RegisterAsync(string email, string username, string firstName, string lastName, string password);

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        Task<User> GetByIdAsync(Guid id);

        /// <summary>
        /// Updates a user's profile
        /// </summary>
        Task<User> UpdateProfileAsync(Guid id, string firstName, string lastName, string username);

        /// <summary>
        /// Changes a user's password
        /// </summary>
        Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword);

        /// <summary>
        /// Verifies a user's email
        /// </summary>
        Task VerifyEmailAsync(Guid id, string verificationToken);
    }
}