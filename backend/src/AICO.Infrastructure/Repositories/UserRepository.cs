using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AICO.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AicoDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUsernameInUseAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetByWebsiteIdAsync(Guid websiteId)
        {
            return await _dbSet
                .Where(u => u.WebsiteId == websiteId)
                .ToListAsync();
        }

        public async Task UpdateLastLoginDateAsync(Guid userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.UpdateLastLoginDate(); // Assuming this method exists in User class
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetEmailVerifiedAsync(Guid userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.VerifyEmail();
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByVerificationTokenAsync(string token)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.VerificationToken == token);
        }

        public async Task SetVerificationTokenAsync(Guid userId, string token)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.VerificationToken = token;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveVerificationTokenAsync(Guid userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.VerificationToken = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserAssignedVariantDto?> GetUserAbTestVariantAsync(string userId, Guid abTestId)
        {
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                // Handle invalid userId format, perhaps log an error or return null
                return null;
            }

            // This is a placeholder implementation. 
            // The actual logic to determine which variant a user is assigned to is missing.
            // This is a placeholder implementation. 
            // The actual logic to determine which variant a user is assigned to is missing.
            // This implementation currently returns the first variant of the A/B test.
            var variant = await _context.Variants
                .FirstOrDefaultAsync(v => v.AbTestId == abTestId);

            if (variant == null)
            {
                return null;
            }

            return new UserAssignedVariantDto { Variant = variant };
        }

        public Task AssignUserToAbTestVariantAsync(string userId, Guid abTestId, Guid variantId)
        {
            throw new NotImplementedException();
        }
    }
}