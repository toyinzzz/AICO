using AICO.Infrastructure.ExternalServices;
using Moq;
using Stripe;
using Xunit;
using AppSubscription = AICO.Application.Interfaces.ExternalServices.Subscription;
using SubscriptionRequest = AICO.Application.Interfaces.ExternalServices.SubscriptionRequest;
using PaymentRequest = AICO.Domain.Interfaces.Services.PaymentRequest;

namespace AICO.UnitTests.Infrastructure.Services
{
    public class StripePaymentServiceTests
    {
        private readonly Mock<PaymentIntentService> _mockPaymentIntentService;
        private readonly Mock<CustomerService> _mockCustomerService;
        private readonly AICO.Application.Interfaces.ExternalServices.IPaymentService _paymentService;

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
            var result = await _paymentService.ProcessPaymentAsync(request.UserId.ToString(), request.Amount, request.Currency);

            // Assert
            Assert.True(result);
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
            var result = await _paymentService.ProcessPaymentAsync(request.UserId.ToString(), request.Amount, request.Currency);

            // Assert
            Assert.False(result);
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

            var subscription = new AppSubscription(
                Id: Guid.NewGuid(),
                UserId: Guid.NewGuid(),
                PlanId: "price_test_plan",
                Status: "active",
                StartDate: DateTime.UtcNow,
                EndDate: DateTime.UtcNow.AddMonths(1),
                Amount: 99.99m
            );

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