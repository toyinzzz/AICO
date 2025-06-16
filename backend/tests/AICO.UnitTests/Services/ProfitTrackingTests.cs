using AICO.Application.Interfaces.ExternalServices;
using AICO.Application.Interfaces.Services;
using AICO.Application.Services;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AICO.UnitTests.Services
{
    public class ProfitTrackingTests
    {
        private readonly Mock<IRevenueRepository> _mockRevenueRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IWebsiteService> _mockWebsiteService;
        private readonly IProfitTrackingService _profitTrackingService;

        public ProfitTrackingTests()
        {
            _mockRevenueRepository = new Mock<IRevenueRepository>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockWebsiteService = new Mock<IWebsiteService>();
            
            _profitTrackingService = new ProfitTrackingService(
                _mockRevenueRepository.Object,
                _mockHttpContextAccessor.Object,
                _mockWebsiteService.Object);
        }

        [Fact]
        public async Task TrackRevenue_WithStripeWebhook_ShouldCreateRevenueRecord()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var websiteId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var stripeEvent = new StripeWebhookEvent
            {
                Type = "payment_intent.succeeded",
                Data = new StripePaymentData
                {
                    Id = "pi_123456789",
                    Amount = 9999, // $99.99 in cents
                    Currency = "usd",
                    Metadata = new Dictionary<string, string>
                    {
                        ["session_id"] = sessionId.ToString(),
                        ["variant_id"] = variantId.ToString(),
                        ["website_id"] = websiteId.ToString(),
                        ["user_id"] = userId.ToString()
                    }
                }
            };

            // Set up mock for website service to validate ownership
            _mockWebsiteService
                .Setup(s => s.ValidateOwnershipAsync(websiteId, userId))
                .ReturnsAsync(true);

            // Set up mock for repository to capture the added revenue
            Revenue capturedRevenue = null;
            _mockRevenueRepository
                .Setup(r => r.AddAsync(It.IsAny<Revenue>()))
                .Callback<Revenue>(r => capturedRevenue = r)
                .Returns(Task.CompletedTask);

            // Act
            var revenue = await _profitTrackingService.ProcessStripeWebhookAsync(stripeEvent);

            // Assert
            Assert.NotNull(revenue);
            Assert.Equal(sessionId, revenue.SessionId);
            Assert.Equal(variantId, revenue.VariantId);
            Assert.Equal(99.99m, revenue.Amount);
            Assert.Equal("USD", revenue.Currency);
            Assert.Equal(websiteId, revenue.WebsiteId);
            Assert.Equal(userId, revenue.UserId);
            Assert.Equal("pi_123456789", revenue.TransactionId);
            
            // Verify repository was called with the correct revenue object
            _mockRevenueRepository.Verify(r => r.AddAsync(It.IsAny<Revenue>()), Times.Once);
            
            // Verify website service was called to validate ownership
            _mockWebsiteService.Verify(s => s.ValidateOwnershipAsync(websiteId, userId), Times.Once);
        }

        [Fact]
        public void CalculateMCP_WithValidData_ShouldReturnCorrectValue()
        {
            // Arrange
            var controlRevenue = 1000m;
            var controlConversions = 100;
            var variantRevenue = 1200m;
            var variantConversions = 110;

            // Act
            var mcp = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

            // Assert
            // MCP = (Variant Revenue/Conversions - Control Revenue/Conversions) / (Control Revenue/Conversions) * 100
            // = (1200/110 - 1000/100) / (1000/100) * 100
            // = (10.91 - 10) / 10 * 100 = 9.1%
            Assert.True(Math.Abs(mcp - 9.09m) < 0.1m, $"Expected ~9.09%, got {mcp}%");
        }

        [Theory]
        [InlineData(1000, 100, 1500, 120, 25.0)] // 25% improvement
        [InlineData(1000, 100, 800, 90, -11.11)] // 11.11% decrease
        [InlineData(1000, 100, 1000, 100, 0)] // No change
        public void CalculateROI_WithVariousScenarios_ShouldReturnCorrectPercentage(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, decimal expectedROI)
        {
            // Act
            var roi = _profitTrackingService.CalculateROI(controlRevenue, controlConversions, variantRevenue, variantConversions);

            // Assert
            Assert.True(Math.Abs(roi - expectedROI) < 0.1m, $"Expected {expectedROI}%, got {roi}%");
        }

        [Fact]
        public async Task GenerateRevenueReport_WithDateRange_ShouldReturnAggregatedData()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;
            var abTestId = Guid.NewGuid();

            // Act
            var report = await _profitTrackingService.GenerateRevenueReportAsync(abTestId, startDate, endDate);

            // Assert
            Assert.NotNull(report);
            Assert.Equal(abTestId, report.AbTestId);
            Assert.Equal(startDate.Date, report.StartDate.Date);
            Assert.Equal(endDate.Date, report.EndDate.Date);
            Assert.True(report.TotalRevenue >= 0);
            Assert.True(report.TotalConversions >= 0);
        }

        [Theory]
        [InlineData("USD", 1000.00, 1100.00)]
        [InlineData("EUR", 850.00, 935.00)]
        [InlineData("GBP", 750.00, 825.00)]
        public void CalculateMCP_WithDifferentCurrencies_ShouldNormalizeCorrectly(string currency, decimal controlRevenue, decimal variantRevenue)
        {
            // Arrange
            var controlConversions = 100;
            var variantConversions = 100;

            // Act
            var result = _profitTrackingService.CalculateMCPWithCurrency(controlRevenue, controlConversions, variantRevenue, variantConversions, currency);

            // Assert
            Assert.True(Math.Abs(result - 10.0m) < 0.1m, "MCP should be consistent across currencies when normalized");
        }

        [Fact]
        public void CalculateMCP_WithMultipleVariants_ShouldCalculateCorrectly()
        {
            // Arrange
            var variants = new List<VariantData>
            {
                new VariantData { Revenue = 1000m, Conversions = 100, IsControl = true },
                new VariantData { Revenue = 1100m, Conversions = 110, IsControl = false },
                new VariantData { Revenue = 1200m, Conversions = 120, IsControl = false }
            };

            // Act
            var results = _profitTrackingService.CalculateMCPForMultipleVariants(variants);

            // Assert
            Assert.Equal(2, results.Count); // Should have MCP for 2 non-control variants
            Assert.All(results, r => Assert.True(r.MCP > 0)); // All variants should show improvement
        }

        [Fact]
        public void CalculateMCP_WithInsufficientSampleSize_ShouldReturnNull()
        {
            // Arrange - sample sizes below statistical significance threshold
            var controlRevenue = 100m;
            var controlConversions = 5; // Too few conversions
            var variantRevenue = 120m;
            var variantConversions = 6;
        
            // Act
            var result = _profitTrackingService.CalculateMCPWithSignificance(controlRevenue, controlConversions, variantRevenue, variantConversions);
        
            // Assert
            Assert.Null(result); // Should return null for insufficient data
        }

        [Fact]
        public void CalculateMCP_WithSufficientSampleSize_ShouldReturnValue()
        {
            // Arrange - sample sizes above statistical significance threshold
            var controlRevenue = 10000m;
            var controlConversions = 1000; // Sufficient conversions
            var variantRevenue = 11000m;
            var variantConversions = 1100;
        
            // Act
            var result = _profitTrackingService.CalculateMCPWithSignificance(controlRevenue, controlConversions, variantRevenue, variantConversions);
        
            // Assert
            Assert.NotNull(result);
            Assert.True(result > 0);
        }

        [Theory]
        [InlineData(0, 100, 1000, 100)] // Zero control revenue
        [InlineData(1000, 0, 1000, 100)] // Zero control conversions - should throw
        [InlineData(1000, 100, 0, 100)] // Zero variant revenue
        [InlineData(1000, 100, 1000, 0)] // Zero variant conversions - should throw
        public void CalculateMCP_WithZeroValues_ShouldHandleCorrectly(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            if (controlConversions == 0 || variantConversions == 0)
            {
                // Should throw ArgumentException for division by zero
                Assert.Throws<ArgumentException>(() => 
                    _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions));
            }
            else
            {
                // Should handle zero revenue gracefully
                var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
                Assert.True(result >= -100); // MCP shouldn't be less than -100%
            }
        }

        [Theory]
        [InlineData(-1000, 100, 1000, 100)] // Negative control revenue
        [InlineData(1000, 100, -1000, 100)] // Negative variant revenue
        [InlineData(1000, -100, 1000, 100)] // Negative control conversions
        [InlineData(1000, 100, 1000, -100)] // Negative variant conversions
        public void CalculateMCP_WithNegativeValues_ShouldThrowArgumentException(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions));
        }

        [Theory]
        [InlineData(decimal.MaxValue, 1, decimal.MaxValue, 1)] // Very large numbers
        [InlineData(0.01m, 1000000, 0.02m, 1000000)] // Very small revenue per conversion
        public void CalculateMCP_WithExtremeValues_ShouldNotOverflow(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            // Act
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

            // Assert
            Assert.True(decimal.IsFinite(result), "MCP calculation should not result in infinity or NaN");
        }

        [Fact]
        public void CalculateMCP_WithPrecisionTest_ShouldRoundCorrectly()
        {
            // Arrange - values that result in repeating decimals
            var controlRevenue = 1000m;
            var controlConversions = 3;
            var variantRevenue = 1001m;
            var variantConversions = 3;

            // Act
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

            // Assert
            Assert.True(Math.Round(result, 4) == result, "MCP should be rounded to 4 decimal places");
        }

        [Theory]
        [InlineData(1000, 100, 500, 100, -50.0)] // 50% decrease
        [InlineData(1000, 100, 2000, 100, 100.0)] // 100% increase
        [InlineData(1000, 100, 1000, 200, -50.0)] // Same revenue, double conversions
        public void CalculateMCP_WithVariousScenarios_ShouldReturnExpectedPercentage(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions, decimal expectedMCP)
        {
            // Act
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);

            // Assert
            Assert.True(Math.Abs(result - expectedMCP) < 0.1m, $"Expected {expectedMCP}%, got {result}%");
        }

        // Add to ProfitTrackingTests class
        [Theory]
        [InlineData(decimal.MinValue, 1, 1000, 100)] // Minimum decimal value
        [InlineData(1000, int.MaxValue, 1100, int.MaxValue)] // Maximum int values
        public void CalculateMCP_WithBoundaryValues_ShouldHandleCorrectly(decimal controlRevenue, int controlConversions, decimal variantRevenue, int variantConversions)
        {
            // Test boundary conditions
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
            Assert.True(decimal.IsFinite(result));
        }

        [Fact]
        public void CalculateMCP_WithVerySmallDifferences_ShouldDetectMinimalChanges()
        {
            // Test precision with very small differences
            var result = _profitTrackingService.CalculateMCP(1000.0000m, 100, 1000.0001m, 100);
            Assert.True(Math.Abs(result - 0.0001m) < 0.00001m);
        }
        
        [Fact]
        public void CalculateMCP_WithZeroControlRPC_ShouldReturnCappedValue()
        {
            // Arrange: Control has zero RPC (revenue per conversion)
            decimal controlRevenue = 0;
            int controlConversions = 100;
            decimal variantRevenue = 1000;
            int variantConversions = 100;
            
            // Act
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
            
            // Assert: Should return capped value (1000%) instead of MaxValue
            Assert.Equal(1000m, result);
        }
        
        [Fact]
        public void CalculateMCP_WithBothZeroRPC_ShouldReturnZero()
        {
            // Arrange: Both control and variant have zero RPC
            decimal controlRevenue = 0;
            int controlConversions = 100;
            decimal variantRevenue = 0;
            int variantConversions = 100;
            
            // Act
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
            
            // Assert: Should return 0 when both have zero RPC
            Assert.Equal(0m, result);
        }
        
        [Fact]
        public void CalculateMCP_WithExtremeValues_ShouldCapResults()
        {
            // Arrange: Extreme improvement (would normally cause very large value)
            decimal controlRevenue = 0.0001m;
            int controlConversions = 100;
            decimal variantRevenue = 10000m;
            int variantConversions = 100;
            
            // Act
            var result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
            
            // Assert: Should cap at 1000%
            Assert.Equal(1000m, result);
            
            // Arrange: Extreme decline (would normally cause very negative value)
            controlRevenue = 10000m;
            variantRevenue = 0.0001m;
            
            // Act
            result = _profitTrackingService.CalculateMCP(controlRevenue, controlConversions, variantRevenue, variantConversions);
            
            // Assert: Should cap at -1000%
            Assert.Equal(-1000m, result);
        }
    }
}