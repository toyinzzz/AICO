using System.Threading.Tasks;

namespace AICO.Application.Interfaces.Services
{
    public interface ICurrencyConversionService
    {
        Task<decimal> GetConversionRateAsync(string fromCurrency, string toCurrency);
    }
}