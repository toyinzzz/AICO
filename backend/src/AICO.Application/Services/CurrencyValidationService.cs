using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs.ValidationResults;
using Microsoft.Extensions.Logging;

namespace AICO.Application.Services;

/// <summary>
/// Service for currency validation
/// Handles currency code validation, amount normalization, and conversion
/// </summary>
public class CurrencyValidationService : ICurrencyValidationService
{
    private readonly ILogger<CurrencyValidationService> _logger;
    private readonly HashSet<string> _supportedCurrencies;
    private readonly Dictionary<string, double> _exchangeRates;

    public CurrencyValidationService(ILogger<CurrencyValidationService> logger)
    {
        _logger = logger;
        _supportedCurrencies = new HashSet<string>
        {
            "USD", "EUR", "GBP", "JPY", "CAD", "AUD", "CHF", "CNY", "SEK", "NZD",
            "MXN", "SGD", "HKD", "NOK", "TRY", "RUB", "INR", "BRL", "ZAR", "KRW"
        };
        
        // Simplified exchange rates (in production, these would come from a real-time service)
        _exchangeRates = new Dictionary<string, double>
        {
            { "USD", 1.0 },
            { "EUR", 0.85 },
            { "GBP", 0.73 },
            { "JPY", 110.0 },
            { "CAD", 1.25 },
            { "AUD", 1.35 },
            { "CHF", 0.92 },
            { "CNY", 6.45 },
            { "SEK", 8.5 },
            { "NZD", 1.42 },
            { "MXN", 20.0 },
            { "SGD", 1.35 },
            { "HKD", 7.8 },
            { "NOK", 8.6 },
            { "TRY", 8.5 },
            { "RUB", 75.0 },
            { "INR", 74.0 },
            { "BRL", 5.2 },
            { "ZAR", 14.5 },
            { "KRW", 1180.0 }
        };
    }

    public CurrencyValidationResult ValidateCurrency(string currency)
    {
        try
        {
            _logger.LogInformation("Validating currency: {Currency}", currency);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation
            if (string.IsNullOrWhiteSpace(currency))
                validationErrors.Add("Currency code cannot be empty");
            else if (currency.Length != 3)
                validationErrors.Add("Currency code must be exactly 3 characters");
            else if (!_supportedCurrencies.Contains(currency.ToUpper()))
                validationErrors.Add($"Currency '{currency}' is not supported");

            var normalizedCurrency = currency?.ToUpper() ?? string.Empty;
            var isSupported = _supportedCurrencies.Contains(normalizedCurrency);
            var exchangeRate = isSupported ? _exchangeRates.GetValueOrDefault(normalizedCurrency, 1.0) : 0.0;

            var result = new CurrencyValidationResult
            {
                IsValid = !validationErrors.Any(),
                IsSupported = isSupported,
                Currency = normalizedCurrency,
                ExchangeRate = exchangeRate,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                SupportedCurrencies = _supportedCurrencies.ToList(),
                NormalizedCurrency = normalizedCurrency,
                NormalizedAmount = 0,
                ConversionRate = exchangeRate,
                TargetCurrency = "USD",
                ConversionRates = new Dictionary<string, double> { { normalizedCurrency, exchangeRate } },
                NormalizedAmounts = new Dictionary<string, decimal> { { normalizedCurrency, 0 } }
            };

            _logger.LogInformation("Currency validation completed. IsValid: {IsValid}, IsSupported: {IsSupported}", 
                result.IsValid, result.IsSupported);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during currency validation");
            throw;
        }
    }

    public async Task<CurrencyValidationResult> ValidateCurrencyAsync(string currencyCode, decimal amount)
    {
        try
        {
            _logger.LogInformation("Async validating currency: {CurrencyCode}, amount: {Amount}", currencyCode, amount);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();

            // Basic validation
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                validationErrors.Add("Currency code is required");
                currencyCode = "USD"; // Default fallback
            }
            else
            {
                currencyCode = currencyCode.Trim().ToUpper();
                if (currencyCode.Length != 3)
                    validationErrors.Add("Currency code must be exactly 3 characters");
            }

            // Amount validation
            if (amount < 0)
            {
                validationErrors.Add("Amount cannot be negative");
                amount = Math.Abs(amount); // Normalize to positive
                validationMessages.Add("Negative amount was normalized to positive value");
            }

            if (amount == 0)
                validationMessages.Add("Zero amount - ensure this is intentional for your test");

            // Currency support validation
            var isSupported = _supportedCurrencies.Contains(currencyCode);
            if (!isSupported)
            {
                validationErrors.Add($"Currency '{currencyCode}' is not currently supported");
                validationMessages.Add($"Supported currencies: {string.Join(", ", _supportedCurrencies.Take(10))}...");
            }

            // Get exchange rate
            var exchangeRate = isSupported ? _exchangeRates.GetValueOrDefault(currencyCode, 1.0) : 0.0;
            var usdAmount = isSupported ? amount * (decimal)exchangeRate : 0;

            // Business rule validations
            if (usdAmount > 100000)
                validationMessages.Add("High-value transaction detected - additional verification may be required");

            // Simulate currency conversion to multiple targets
            var conversionRates = new Dictionary<string, double>();
            var normalizedAmounts = new Dictionary<string, decimal>();
            
            foreach (var targetCurrency in new[] { "USD", "EUR", "GBP" })
            {
                if (_exchangeRates.ContainsKey(targetCurrency))
                {
                    var targetRate = _exchangeRates[targetCurrency];
                    var conversionRate = isSupported ? exchangeRate / targetRate : 0;
                    conversionRates[targetCurrency] = conversionRate;
                    normalizedAmounts[targetCurrency] = isSupported ? amount * (decimal)conversionRate : 0;
                }
            }

            var result = new CurrencyValidationResult
            {
                IsValid = !validationErrors.Any(),
                IsSupported = isSupported,
                Currency = currencyCode,
                ExchangeRate = exchangeRate,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                SupportedCurrencies = _supportedCurrencies.ToList(),
                NormalizedCurrency = currencyCode,
                NormalizedAmount = amount,
                ConversionRate = exchangeRate,
                TargetCurrency = "USD",
                ConversionRates = conversionRates,
                NormalizedAmounts = normalizedAmounts
            };

            _logger.LogInformation("Async currency validation completed. Valid: {IsValid}, Supported: {IsSupported}, USD Amount: {UsdAmount}", 
                result.IsValid, result.IsSupported, usdAmount);

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during async currency validation");
            throw;
        }
    }

    public async Task<CurrencyValidationResult> ValidateCurrencyAsync(List<(decimal amount, string currency)> amounts, string targetCurrency)
    {
        try
        {
            _logger.LogInformation("Validating {Count} currency amounts for target currency {TargetCurrency}", amounts?.Count ?? 0, targetCurrency);

            var validationErrors = new List<string>();
            var validationMessages = new List<string>();
            var conversionRates = new Dictionary<string, double>();
            var normalizedAmounts = new Dictionary<string, decimal>();

            if (amounts == null || !amounts.Any())
            {
                validationErrors.Add("At least one amount is required");
                return new CurrencyValidationResult
                {
                    IsValid = false,
                    ValidationErrors = validationErrors,
                    ValidationMessages = validationMessages
                };
            }

            // Validate target currency
            if (string.IsNullOrWhiteSpace(targetCurrency))
            {
                validationErrors.Add("Target currency is required");
                targetCurrency = "USD";
            }
            else
            {
                targetCurrency = targetCurrency.ToUpper();
                if (!_supportedCurrencies.Contains(targetCurrency))
                    validationErrors.Add($"Target currency '{targetCurrency}' is not supported");
            }

            // Validate each amount and currency
            foreach (var (amount, currency) in amounts)
            {
                if (amount < 0)
                    validationErrors.Add($"Amount {amount} cannot be negative");
                
                if (string.IsNullOrWhiteSpace(currency))
                    validationErrors.Add("Currency code cannot be empty");
                else
                {
                    var normalizedCurrency = currency.ToUpper();
                    if (!_supportedCurrencies.Contains(normalizedCurrency))
                        validationErrors.Add($"Currency '{currency}' is not supported");
                    else
                    {
                        var rate = GetExchangeRate(normalizedCurrency, targetCurrency);
                        conversionRates[normalizedCurrency] = rate;
                        normalizedAmounts[normalizedCurrency] = amount * (decimal)rate;
                    }
                }
            }

            var result = new CurrencyValidationResult
            {
                IsValid = !validationErrors.Any(),
                IsSupported = _supportedCurrencies.Contains(targetCurrency),
                Currency = targetCurrency,
                ExchangeRate = 1.0,
                ValidationErrors = validationErrors,
                ValidationMessages = validationMessages,
                SupportedCurrencies = _supportedCurrencies.ToList(),
                NormalizedCurrency = targetCurrency,
                NormalizedAmount = normalizedAmounts.Values.Sum(),
                ConversionRate = 1.0,
                TargetCurrency = targetCurrency,
                ConversionRates = conversionRates,
                NormalizedAmounts = normalizedAmounts
            };

            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during multi-currency validation");
            throw;
        }
    }

    /// <summary>
    /// Get current exchange rate for a currency pair
    /// </summary>
    public double GetExchangeRate(string fromCurrency, string toCurrency = "USD")
    {
        try
        {
            fromCurrency = fromCurrency?.ToUpper() ?? "USD";
            toCurrency = toCurrency?.ToUpper() ?? "USD";

            if (fromCurrency == toCurrency)
                return 1.0;

            if (!_exchangeRates.ContainsKey(fromCurrency) || !_exchangeRates.ContainsKey(toCurrency))
                return 0.0;

            var fromRate = _exchangeRates[fromCurrency];
            var toRate = _exchangeRates[toCurrency];

            return fromRate / toRate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting exchange rate for {FromCurrency} to {ToCurrency}", fromCurrency, toCurrency);
            return 0.0;
        }
    }

    /// <summary>
    /// Convert amount from one currency to another
    /// </summary>
    public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency = "USD")
    {
        try
        {
            var rate = GetExchangeRate(fromCurrency, toCurrency);
            return rate > 0 ? amount * (decimal)rate : 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting currency from {FromCurrency} to {ToCurrency}", fromCurrency, toCurrency);
            return 0;
        }
    }

    /// <summary>
    /// Check if a currency is supported
    /// </summary>
    public bool IsCurrencySupported(string currencyCode)
    {
        return !string.IsNullOrWhiteSpace(currencyCode) && 
               _supportedCurrencies.Contains(currencyCode.ToUpper());
    }

    /// <summary>
    /// Get all supported currencies
    /// </summary>
    public IEnumerable<string> GetSupportedCurrencies()
    {
        return _supportedCurrencies.OrderBy(c => c);
    }
}