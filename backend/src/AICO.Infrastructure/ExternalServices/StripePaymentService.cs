using AICO.Application.Interfaces.ExternalServices;
using Stripe;

namespace AICO.Infrastructure.ExternalServices
{
    public class StripePaymentService : IPaymentService
    {
        private PaymentIntentService _paymentIntentService ;
        private readonly CustomerService _customerService;

        public StripePaymentService(PaymentIntentService paymentIntentService, CustomerService customerService)
        {
            _paymentIntentService = paymentIntentService;
            _customerService = customerService;
        }

        public Task<bool> ProcessPaymentAsync(string userId, decimal amount, string currency)
        {
            throw new System.NotImplementedException();
        }

        public Task<Application.Interfaces.ExternalServices.Subscription> CreateSubscriptionAsync(SubscriptionRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}