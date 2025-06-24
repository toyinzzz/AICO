using AICO.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AICO.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User : BaseEntity, IAuditableEntity
    {
        /// <summary>
        /// User's email address (unique)
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; private set; }

        /// <summary>
        /// User's username
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Username { get; private set; }

        /// <summary>
        /// User's first name
        /// </summary>
        [MaxLength(100)]
        public string FirstName { get; private set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [MaxLength(100)]
        public string LastName { get; private set; }

        /// <summary>
        /// Hashed password
        /// </summary>
        [Required]
        public string PasswordHash { get; private set; }

        /// <summary>
        /// Salt used for password hashing
        /// </summary>
        [Required]
        public string PasswordSalt { get; private set; }

        /// <summary>
        /// Foreign key for the Website this user is primarily associated with (optional)
        /// </summary>
        public Guid? WebsiteId { get; private set; }

        /// <summary>
        /// Navigation property for the Website
        /// </summary>
        public Website? Website { get; private set; }

        /// <summary>
        /// User's role (e.g., Admin, User)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Role { get; private set; }

        /// <summary>
        /// Flag indicating if the user's email is verified
        /// </summary>
        public bool IsEmailVerified { get; private set; }

        /// <summary>
        /// Date when the user last logged in
        /// </summary>
        public DateTime? LastLoginDate { get; private set; }

        /// <summary>
        /// Collection of websites owned by the user
        /// </summary>
        public virtual ICollection<Website> Websites { get; private set; }

        /// <summary>
        /// Token for email verification (optional, can be null)
        /// </summary>
        public string? VerificationToken { get; internal set; }

        // Private constructor for EF Core
        private User()
        {
            Websites = new List<Website>();
        }

        // Static factory method for creating a new user
        public static User Create(
            string email,
            string username,
            string firstName,
            string lastName,
            string passwordHash,
            string passwordSalt,
            string role = "User")
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required", nameof(passwordHash));

            if (string.IsNullOrWhiteSpace(passwordSalt))
                throw new ArgumentException("Password salt is required", nameof(passwordSalt));

            return new User
            {
                Email = email.Trim(),
                Username = username.Trim(),
                FirstName = firstName?.Trim(),
                LastName = lastName?.Trim(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role,
                IsEmailVerified = false
            };
        }

        // Method for updating user profile information
        public void UpdateProfile(string firstName, string lastName, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));

            Username = username.Trim();
            FirstName = firstName?.Trim();
            LastName = lastName?.Trim();
            MarkAsUpdated();
        }

        // Method for updating user's password
        public void UpdatePassword(string passwordHash, string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required", nameof(passwordHash));

            if (string.IsNullOrWhiteSpace(passwordSalt))
                throw new ArgumentException("Password salt is required", nameof(passwordSalt));

            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            MarkAsUpdated();
        }

        // Method for marking email as verified
        public void VerifyEmail()
        {
            IsEmailVerified = true;
            MarkAsUpdated();
        }

        // Method for updating the last login date
        public void UpdateLastLoginDate()
        {
            LastLoginDate = DateTime.UtcNow;
            MarkAsUpdated();
        }

        // Method for updating user's role (admin only operation)
        public void UpdateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required", nameof(role));

            Role = role;
            MarkAsUpdated();
        }
    }
}