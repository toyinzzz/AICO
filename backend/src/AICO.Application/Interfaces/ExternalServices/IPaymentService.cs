using System.Threading.Tasks;

namespace AICO.Application.Interfaces.ExternalServices
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(string userId, decimal amount, string currency);
        Task<Subscription> CreateSubscriptionAsync(SubscriptionRequest request);
        // Add other payment related methods here, e.g., refunds, subscriptions
    }

    public record SubscriptionRequest(
        Guid UserId,
        string PlanId,
        string PaymentMethodId
    );

    public record Subscription(
        Guid Id,
        Guid UserId,
        string PlanId,
        string Status,
        DateTime StartDate,
        DateTime? EndDate,
        decimal Amount
    );
}