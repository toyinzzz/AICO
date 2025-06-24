using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AICO.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            // Replace with a proper password hashing and verification mechanism
            if (user.PasswordHash != password)
            {
                return null;
            }

            return user;
        }

        public async Task<User> RegisterAsync(string email, string username, string firstName, string lastName, string password)
        {
            if (await _userRepository.GetByEmailAsync(email) != null)
            {
                throw new Exception("Email \"" + email + "\" is already taken");
            }

            if (await _userRepository.GetByUsernameAsync(username) != null)
            {
                throw new Exception("Username \"" + username + "\" is already taken");
            }

            // Replace with a proper password hashing mechanism
            // TODO: Implement proper password hashing and salt generation
            var passwordSalt = "temp_salt"; // This should be generated properly
            var user = User.Create(
                email,
                username,
                firstName,
                lastName,
                password, // This should be hashed
                passwordSalt
            );

            await _userRepository.AddAsync(user);

            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> UpdateProfileAsync(Guid id, string firstName, string lastName, string username)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.UpdateProfile(firstName, lastName, username);

            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Replace with a proper password hashing and verification mechanism
            if (user.PasswordHash != currentPassword)
            {
                throw new Exception("Incorrect password");
            }

            // Replace with a proper password hashing mechanism
            // TODO: Implement proper password hashing and salt generation
            var passwordSalt = "temp_salt"; // This should be generated properly
            user.UpdatePassword(newPassword, passwordSalt); // Store new hashed password

            await _userRepository.UpdateAsync(user);
        }

        public async Task VerifyEmailAsync(Guid id, string verificationToken)
        {
            // This is a placeholder. In a real application, you would have a verification token
            // stored against the user and you would verify it here.
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.VerifyEmail();

            await _userRepository.UpdateAsync(user);
        }
    }
}