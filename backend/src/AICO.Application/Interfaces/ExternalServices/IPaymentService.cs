using System;
using System.Threading.Tasks;

namespace AICO.Application.Interfaces.ExternalServices
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task<Subscription> CreateSubscriptionAsync(SubscriptionRequest request);
        Task<Subscription> UpdateSubscriptionAsync(Guid subscriptionId, SubscriptionUpdateRequest request);
        Task CancelSubscriptionAsync(Guid subscriptionId);
        Task<PaymentMethod[]> GetPaymentMethodsAsync(Guid userId);
        Task<Invoice[]> GetInvoicesAsync(Guid userId);
    }

    public record PaymentRequest(
        Guid UserId,
        decimal Amount,
        string Currency,
        string PaymentMethodId,
        string Description
    );

    public record PaymentResult(
        bool Success,
        string TransactionId,
        string Status,
        string ErrorMessage
    );

    public record SubscriptionRequest(
        Guid UserId,
        string PlanId,
        string PaymentMethodId
    );

    public record SubscriptionUpdateRequest(
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

    public record PaymentMethod(
        string Id,
        string Type,
        string Last4,
        string Brand,
        bool IsDefault
    );

    public record Invoice(
        string Id,
        decimal Amount,
        string Currency,
        string Status,
        DateTime CreatedDate,
        DateTime? PaidDate
    );
}