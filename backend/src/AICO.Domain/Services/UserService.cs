using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Services;

namespace AICO.Domain.Services;

public class UserService : IUserService
{
    public Task<User> AuthenticateAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateUserAsync(string email, string firstName, string lastName, string company = null)
    {
        throw new NotImplementedException("User management will be implemented in Phase 2");
    }

    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException("User management will be implemented in Phase 2");
    }

    public Task<User> RegisterAsync(string email, string username, string firstName, string lastName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateProfileAsync(Guid id, string firstName, string lastName, string username)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateUserAsync(Guid userId, string firstName, string lastName, string company = null)
    {
        throw new NotImplementedException("User management will be implemented in Phase 2");
    }

    public Task<bool> ValidateUserAsync(Guid userId)
    {
        throw new NotImplementedException("User validation will be implemented in Phase 2");
    }

    public Task VerifyEmailAsync(Guid id, string verificationToken)
    {
        throw new NotImplementedException();
    }
}