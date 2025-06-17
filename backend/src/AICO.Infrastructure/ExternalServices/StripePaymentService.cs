using AICO.Application.Interfaces.ExternalServices;
using System.Threading.Tasks;

namespace AICO.Infrastructure.ExternalServices
{
    public class StripePaymentService : AICO.Application.Interfaces.ExternalServices.IPaymentService
    {
        public Task<bool> ProcessPaymentAsync(string userId, decimal amount, string currency)
        {
            throw new System.NotImplementedException();
        }
    }
}