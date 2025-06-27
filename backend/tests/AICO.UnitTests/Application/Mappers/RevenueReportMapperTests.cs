using AICO.Application.Mappers;
using AICO.Domain.DTOs;
using Xunit;

namespace AICO.UnitTests.Application.Mappers
{
    public class RevenueReportMapperTests
    {
        private readonly RevenueReportMapper _mapper;

        public RevenueReportMapperTests()
        {
            _mapper = new RevenueReportMapper();
        }

        [Fact]
        public void Map_WithValidSource_ShouldMapCorrectly()
        {
            // Arrange
            var source = new RevenueReportSource
            {
                CampaignId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                Metrics = new RevenueMetrics
                {
                    TotalRevenue = 1000,
                    Conversions = 100,
                    AverageOrderValue = 10,
                    RevenueByVariant = new Dictionary<Guid, decimal>
                    {
                        { Guid.NewGuid(), 500 },
                        { Guid.NewGuid(), 500 }
                    }
                },
                OverallMcp = 0.5m,
                WinningVariant = Guid.NewGuid().ToString()
            };

            // Act
            var result = _mapper.Map(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.CampaignId, result.AbTestId);
            Assert.Equal(source.StartDate, result.StartDate);
            Assert.Equal(source.EndDate, result.EndDate);
            Assert.Equal(source.Metrics.TotalRevenue, result.TotalRevenue);
            Assert.Equal(source.Metrics.Conversions, result.TotalConversions);
            Assert.Equal(source.Metrics.AverageOrderValue, result.AverageOrderValue);
            Assert.Equal(source.Metrics.RevenueByVariant.Count, result.RevenueByVariant.Count);
            Assert.Equal(source.OverallMcp, result.OverallMCP);
            Assert.Equal(Guid.Parse(source.WinningVariant), result.WinningVariantId);
        }

        [Fact]
        public void Map_WithNullMetrics_ShouldHandleGracefully()
        {
            // Arrange
            var source = new RevenueReportSource
            {
                CampaignId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow,
                Metrics = null,
                OverallMcp = 0.5m,
                WinningVariant = Guid.NewGuid().ToString()
            };

            // Act
            var result = _mapper.Map(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.CampaignId, result.AbTestId);
            Assert.Equal(0, result.TotalRevenue);
            Assert.Equal(0, result.TotalConversions);
            Assert.Equal(0, result.AverageOrderValue);
            Assert.Empty(result.RevenueByVariant);
        }


        [Fact]
        public void MapWithNullSource_ShouldReturnNull()
        {
            // Arrange, Act
            var result = _mapper.Map((RevenueReportSource)null);

            // Assert
            Assert.Null(result);
        }
    }
}