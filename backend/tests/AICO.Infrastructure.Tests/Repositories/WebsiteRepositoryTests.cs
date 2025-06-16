using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AICO.Domain.Entities;
using AICO.Infrastructure.Data;
using AICO.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AICO.Infrastructure.Tests.Repositories
{
    public class WebsiteRepositoryTests
    {
        private readonly AicoDbContext _context;
        private readonly WebsiteRepository _repository;

        public WebsiteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AicoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AicoDbContext(options);
            _repository = new WebsiteRepository(_context);
        }

        [Fact]
        public async Task GetByDomainAsync_ReturnsWebsite_WhenDomainExists()
        {
            var website = new Website { Id = Guid.NewGuid(), Domain = "example.com", UserId = Guid.NewGuid() };
            _context.Websites.Add(website);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByDomainAsync("example.com");

            Assert.NotNull(result);
            Assert.Equal("example.com", result.Domain);
        }

        [Fact]
        public async Task GetByDomainAsync_ReturnsNull_WhenDomainDoesNotExist()
        {
            var result = await _repository.GetByDomainAsync("notfound.com");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUserIdAsync_ReturnsWebsites_ForGivenUserId()
        {
            var userId = Guid.NewGuid();
            var website1 = new Website { Id = Guid.NewGuid(), Domain = "a.com", UserId = userId };
            var website2 = new Website { Id = Guid.NewGuid(), Domain = "b.com", UserId = userId };
            _context.Websites.AddRange(website1, website2);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByUserIdAsync(userId);

            Assert.Equal(2, result.Count());
            Assert.All(result, w => Assert.Equal(userId, w.UserId));
        }

        [Fact]
        public async Task ExistsByDomainAsync_ReturnsTrue_IfDomainExists()
        {
            var website = new Website { Id = Guid.NewGuid(), Domain = "exists.com", UserId = Guid.NewGuid() };
            _context.Websites.Add(website);
            await _context.SaveChangesAsync();

            var exists = await _repository.ExistsByDomainAsync("exists.com");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByDomainAsync_ReturnsFalse_IfDomainDoesNotExist()
        {
            var exists = await _repository.ExistsByDomainAsync("missing.com");
            Assert.False(exists);
        }
    }
}

