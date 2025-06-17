using System.Threading.Tasks;

namespace AICO.Application.Interfaces.ExternalServices
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(string userId, decimal amount, string currency);
        // Add other payment related methods here, e.g., refunds, subscriptions
    }
}