using AICO.Domain.DTOs.ValidationResults;

namespace AICO.Application.Interfaces.Services;

/// <summary>
/// Interface for currency validation
/// Handles currency code validation and normalization
/// </summary>
public interface ICurrencyValidationService
{
    /// <summary>
    /// Validate currency inputs and normalization requirements
    /// </summary>
    /// <param name="currency">Currency code to validate</param>
    /// <returns>Currency validation result</returns>
    CurrencyValidationResult ValidateCurrency(string currency);

    /// <summary>
    /// Validate currency inputs and normalization requirements asynchronously
    /// </summary>
    /// <param name="amounts">List of amounts with currencies to validate</param>
    /// <param name="targetCurrency">Target currency for normalization</param>
    /// <returns>Currency validation result</returns>
    Task<CurrencyValidationResult> ValidateCurrencyAsync(List<(decimal amount, string currency)> amounts, string targetCurrency);

    /// <summary>
    /// Validate single currency and amount asynchronously
    /// </summary>
    /// <param name="currency">Currency code to validate</param>
    /// <param name="amount">Amount to validate</param>
    /// <returns>Currency validation result</returns>
    Task<CurrencyValidationResult> ValidateCurrencyAsync(string currency, decimal amount);
}