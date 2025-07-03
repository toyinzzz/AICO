using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly ILogger<CurrencyConversionService> _logger;

        private static readonly Dictionary<string, decimal> ConversionRates = new(StringComparer.OrdinalIgnoreCase)
        {
            { "USD", 1.0m },
            { "EUR", 1.1m },
            { "GBP", 1.3m },
            { "CAD", 0.8m },
            { "AUD", 0.7m },
            { "JPY", 0.009m }
        };

        public CurrencyConversionService(ILogger<CurrencyConversionService> logger)
        { 
            _logger = logger;
        }

        public Task<decimal> GetConversionRateAsync(string fromCurrency, string toCurrency)
        {
            _logger.LogInformation("Attempting to get conversion rate from {FromCurrency} to {ToCurrency}", fromCurrency, toCurrency);

            if (!ConversionRates.TryGetValue(fromCurrency, out var fromRate))
            {
                _logger.LogWarning("Source currency {FromCurrency} not supported.", fromCurrency);
                throw new ArgumentException($"Currency {fromCurrency} is not supported.", nameof(fromCurrency));
            }

            if (!ConversionRates.TryGetValue(toCurrency, out var toRate))
            {
                _logger.LogWarning("Target currency {ToCurrency} not supported.", toCurrency);
                throw new ArgumentException($"Currency {toCurrency} is not supported.", nameof(toCurrency));
            }

            var conversionRate = toRate / fromRate;
            _logger.LogInformation("Successfully calculated conversion rate: {ConversionRate}", conversionRate);

            return Task.FromResult(conversionRate);
        }
    }
}