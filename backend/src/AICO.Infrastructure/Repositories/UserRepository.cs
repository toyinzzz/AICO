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

        public async Task<IEnumerable<User>> GetByWebsiteIdAsync(Guid websiteId)
        {
            return await _dbSet
                .Where(u => u.WebsiteId == websiteId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            // Basic implementation: find user by username. Assumes Username is a property on User entity.
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailInUseAsync(string email)
        {
            // Basic implementation: check if email exists. This is similar to ExistsByEmailAsync.
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUsernameInUseAsync(string username)
        {
            // Basic implementation: check if username exists. Assumes Username is a property on User entity.
            return await _dbSet.AnyAsync(u => u.Username == username);
        }


    }
}