using System;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Logging;
using AICO.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using AICO.Application.Interfaces.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using AICO.Application.Services;
using Moq;
using Xunit;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AICO.UnitTests.Services
{
    public class MCPPerformanceTests
    {
        private readonly IProfitTrackingService _profitTrackingService;

        public MCPPerformanceTests()
        {
            var mockRevenueRepository = new Mock<IRevenueRepository>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockWebsiteService = new Mock<IWebsiteService>();
            var mockLogger = new Mock<ILogger<ProfitTrackingService>>();
            
            _profitTrackingService = new ProfitTrackingService(
                mockRevenueRepository.Object,
                mockHttpContextAccessor.Object,
                mockWebsiteService.Object,
                mockLogger.Object);
        }

    [Fact]
    public void CalculateMCP_WithLargeDataset_ShouldCompleteWithinTimeLimit()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();
        var largeRevenue = 1000000m;
        var largeConversions = 100000;

        // Act
        var result = _profitTrackingService.CalculateMCP(largeRevenue, largeConversions, largeRevenue * 1.1m, largeConversions);
        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 100, "MCP calculation should complete within 100ms");
        Assert.True(Math.Abs(result - 10.0m) < 0.1m);
    }

    [Fact]
    public async Task CalculateMCP_ConcurrentCalculations_ShouldHandleCorrectly()
    {
        // Arrange
        var tasks = new List<Task<decimal>>();

        // Act - Run 100 concurrent MCP calculations
        for (int i = 0; i < 100; i++)
        {
            var task = Task.Run(() => _profitTrackingService.CalculateMCP(1000m, 100, 1100m, 110));
            tasks.Add(task);
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.All(results, r => Assert.True(Math.Abs(r - 10.0m) < 0.1m));
    }
    }
}