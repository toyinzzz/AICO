using System;
using System.Threading.Tasks;
using AICO.Domain.Interfaces.Services;
using AICO.Infrastructure.ExternalServices;
using Moq;
using Stripe;
using Xunit;

namespace AICO.UnitTests.Infrastructure.Services
{
    public class StripePaymentServiceTests
    {
        private readonly Mock<PaymentIntentService> _mockPaymentIntentService;
        private readonly Mock<CustomerService> _mockCustomerService;
        private readonly IStripePaymentService _paymentService;

        public StripePaymentServiceTests()
        {
            _mockPaymentIntentService = new Mock<PaymentIntentService>();
            _mockCustomerService = new Mock<CustomerService>();
            _paymentService = new StripePaymentService(
                _mockPaymentIntentService.Object,
                _mockCustomerService.Object);
        }

        [Fact]
        public async Task ProcessPaymentAsync_WithValidRequest_ShouldReturnSuccess()
        {
            // Arrange
            var request = new PaymentRequest(
                UserId: Guid.NewGuid(),
                Amount: 99.99m,
                Currency: "usd",
                PaymentMethodId: "pm_test_123",
                Description: "Test payment"
            );

            var paymentIntent = new PaymentIntent
            {
                Id = "pi_test_123",
                Status = "succeeded",
                Amount = 9999 // Stripe uses cents
            };

            _mockPaymentIntentService.Setup(x => x.CreateAsync(It.IsAny<PaymentIntentCreateOptions>(), null, default))
                .ReturnsAsync(paymentIntent);

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("pi_test_123", result.TransactionId);
            Assert.Equal("succeeded", result.Status);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task ProcessPaymentAsync_WithFailedPayment_ShouldReturnFailure()
        {
            // Arrange
            var request = new PaymentRequest(
                UserId: Guid.NewGuid(),
                Amount: 99.99m,
                Currency: "usd",
                PaymentMethodId: "pm_test_failed",
                Description: "Test failed payment"
            );

            _mockPaymentIntentService.Setup(x => x.CreateAsync(It.IsAny<PaymentIntentCreateOptions>(), null, default))
                .ThrowsAsync(new StripeException("Your card was declined."));

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("declined", result.ErrorMessage);
        }

        [Fact]
        public async Task CreateSubscriptionAsync_WithValidRequest_ShouldReturnSubscription()
        {
            // Arrange
            var request = new SubscriptionRequest(
                UserId: Guid.NewGuid(),
                PlanId: "price_test_plan",
                PaymentMethodId: "pm_test_123"
            );

            var subscription = new Subscription
            {
                Id = "sub_test_123",
                Status = "active",
                CurrentPeriodStart = DateTime.UtcNow,
                CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
            };

            _mockPaymentIntentService.Setup(x => x.CreateAsync(It.IsAny<PaymentIntentCreateOptions>(), null, default))
                .ReturnsAsync(new PaymentIntent { Status = "succeeded" });

            // Act
            var result = await _paymentService.CreateSubscriptionAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("active", result.Status);
        }

       
    }
}