using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using System.Security.Cryptography;

using System.Text;

namespace AICO.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<User> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var hash = HashPassword(password, user.PasswordSalt);
        if (user.PasswordHash != hash)
            throw new UnauthorizedAccessException("Invalid credentials");

        user.UpdateLastLoginDate();
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task<User> RegisterAsync(string email, string username, string firstName, string lastName, string password)
    {
        if (await _userRepository.IsEmailInUseAsync(email))
            throw new ArgumentException("Email already in use", nameof(email));
        if (await _userRepository.IsUsernameInUseAsync(username))
            throw new ArgumentException("Username already in use", nameof(username));

        var salt = GenerateSalt();
        var hash = HashPassword(password, salt);

        var user = User.Create(email, username, firstName, lastName, hash, salt);
        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");
        return user;
    }

    public async Task<User> UpdateProfileAsync(Guid id, string firstName, string lastName, string username)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.UpdateProfile(firstName, lastName, username);
        await _userRepository.UpdateAsync(user);
        return user;
    }

    public async Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var currentHash = HashPassword(currentPassword, user.PasswordSalt);
        if (user.PasswordHash != currentHash)
            throw new UnauthorizedAccessException("Current password is incorrect");

        var newSalt = GenerateSalt();
        var newHash = HashPassword(newPassword, newSalt);
        user.UpdatePassword(newHash, newSalt);
        await _userRepository.UpdateAsync(user);
    }

    public async Task VerifyEmailAsync(Guid id, string verificationToken)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (!_jwtService.ValidateEmailVerificationToken(verificationToken, out var tokenUserId, out var tokenEmail))
            throw new InvalidOperationException("Invalid or expired verification token");

        if (id != tokenUserId || user.Email != tokenEmail)
            throw new InvalidOperationException("Token does not match user");

        user.VerifyEmail();
        await _userRepository.UpdateAsync(user);
    }

    private static string GenerateSalt()
    {
        var bytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }
}