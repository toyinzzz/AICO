using System;
using AICO.Domain.Entities;
using Xunit;

namespace AICO.Domain.Tests.Entities
{
    public class RevenueTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnRevenue()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var amount = 99.99m;
            var currency = "USD";
            var transactionId = "txn_123456";
            var timestamp = DateTime.UtcNow;

            // Act
            var revenue = Revenue.Create(sessionId, variantId, amount, currency, transactionId, timestamp);

            // Assert
            Assert.NotNull(revenue);
            Assert.Equal(sessionId, revenue.SessionId);
            Assert.Equal(variantId, revenue.VariantId);
            Assert.Equal(amount, revenue.Amount);
            Assert.Equal(currency, revenue.Currency);
            Assert.Equal(transactionId, revenue.TransactionId);
            Assert.Equal(timestamp, revenue.Timestamp);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Create_WithInvalidAmount_ShouldThrowArgumentException(decimal invalidAmount)
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var variantId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Revenue.Create(sessionId, variantId, invalidAmount, "USD", "txn_123", DateTime.UtcNow));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Create_WithInvalidCurrency_ShouldThrowArgumentException(string invalidCurrency)
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var variantId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Revenue.Create(sessionId, variantId, 99.99m, invalidCurrency, "txn_123", DateTime.UtcNow));
        }

        [Fact]
        public void Create_WithFutureTimestamp_ShouldThrowArgumentException()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var variantId = Guid.NewGuid();
            var futureTimestamp = DateTime.UtcNow.AddDays(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                Revenue.Create(sessionId, variantId, 99.99m, "USD", "txn_123", futureTimestamp));
        }
    }
}