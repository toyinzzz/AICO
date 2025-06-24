using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IWebsiteRepository _websiteRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<WebsiteService> _logger;

        public WebsiteService(IWebsiteRepository websiteRepository, IUserRepository userRepository, ILogger<WebsiteService> logger)
        {
            _websiteRepository = websiteRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Website> CreateAsync(string url, string name, string description, string industry, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("User with id {userId} not found", userId);
                throw new Exception("User not found");
            }

            var website = Website.Create(url, name, userId, description, industry);

            await _websiteRepository.AddAsync(website);
            return website;
        }

        public async Task<Website> GetByIdAsync(Guid id)
        {
            return await _websiteRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Website>> GetByUserIdAsync(Guid userId)
        {
            return await _websiteRepository.GetByOwnerIdAsync(userId);
        }

        public async Task<Website> UpdateAsync(Guid id, string name, string description, string industry)
        {
            var website = await _websiteRepository.GetByIdAsync(id);
            if (website == null)
            {
                _logger.LogError("Website with id {id} not found", id);
                throw new Exception("Website not found");
            };

            website.Update(name, description, industry, null); // domain parameter is null for now

            await _websiteRepository.UpdateAsync(website);
            return website;
        }

        public async Task<Website> UpdateUrlAsync(Guid id, string url)
        {
            var website = await _websiteRepository.GetByIdAsync(id);
            if (website == null)
            {
                _logger.LogError("Website with id {id} not found", id);
                throw new Exception("Website not found");
            }

            website.UpdateUrl(url);

            await _websiteRepository.UpdateAsync(website);
            return website;
        }

        public async Task DeleteAsync(Guid id)
        {
            var website = await _websiteRepository.GetByIdAsync(id);
            if (website == null)
            {
                _logger.LogError("Website with id {id} not found", id);
                throw new Exception("Website not found");
            }

            await _websiteRepository.DeleteAsync(website);
        }

        public async Task<bool> ValidateOwnershipAsync(Guid websiteId, Guid userId)
        {
            var website = await _websiteRepository.GetByIdAsync(websiteId);
            return website != null && website.UserId == userId;
        }
    }
}