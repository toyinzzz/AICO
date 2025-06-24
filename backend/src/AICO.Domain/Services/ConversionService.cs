using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace AICO.Domain.Services;

public class ConversionService : IConversionService
{
    private readonly IConversionRepository _conversionRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ILogger<ConversionService> _logger;

    public ConversionService(
        IConversionRepository conversionRepository,
        ISessionRepository sessionRepository,
        ILogger<ConversionService> logger)
    {
        _conversionRepository = conversionRepository;
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    public async Task<Conversion?> RecordConversionAsync(Guid websiteId, string conversionType, string goalName, decimal? value = null, string? currency = null, string? conversionData = null, Guid? sessionId = null)
    {
        if (websiteId == Guid.Empty || string.IsNullOrEmpty(conversionType) || string.IsNullOrEmpty(goalName))
        {
            _logger.LogWarning("Invalid input for recording conversion.");
            return null;
        }

        var conversion = Conversion.Create(websiteId, conversionType, goalName, value, currency, conversionData, sessionId);
        return await _conversionRepository.AddAsync(conversion);
    }

    public async Task<IEnumerable<Conversion>?> GetWebsiteConversionsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _conversionRepository.GetConversionsByWebsiteIdAsync(websiteId, startDate, endDate);
    }

    public async Task<IEnumerable<Conversion>?> GetSessionConversionsAsync(Guid sessionId)
    {
        return await _conversionRepository.GetConversionsBySessionIdAsync(sessionId);
    }

    public async Task<IEnumerable<Conversion>?> GetConversionsByTypeAsync(Guid websiteId, string conversionType, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _conversionRepository.GetConversionsByTypeAsync(websiteId, conversionType, startDate, endDate);
    }

    public async Task<IEnumerable<Conversion>?> GetConversionsByGoalAsync(Guid websiteId, string goalName, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _conversionRepository.GetConversionsByGoalAsync(websiteId, goalName, startDate, endDate);
    }

    public async Task<double> CalculateConversionRateAsync(Guid websiteId, string? goalName = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        if (websiteId == Guid.Empty) return 0.0;

        // Hole die Anzahl der Conversions für die Website (und optional das Ziel)
        int conversionCount = await _conversionRepository.GetCountAsync(websiteId, goalName, startDate, endDate);

        // Hole die Gesamtanzahl der Sessions für den angegebenen Zeitraum
        var sessionStats = await _sessionRepository.GetStatisticsAsync(websiteId, startDate, endDate);
        int totalSessions = sessionStats.TotalSessions;

        if (totalSessions == 0) return 0;
        return (double)conversionCount / totalSessions;
    }

    public async Task<(decimal TotalValue, string? Currency)> CalculateTotalValueAsync(Guid websiteId, string? conversionType = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var conversions = await _conversionRepository.GetConversionsByWebsiteIdAsync(websiteId, startDate, endDate);
        if (conversionType != null)
        {
            conversions = conversions.Where(c => c.ConversionType.Value == conversionType);
        }

        var totalValue = conversions.Sum(c => c.Value ?? 0);
        var currency = conversions.FirstOrDefault(c => !string.IsNullOrEmpty(c.Currency))?.Currency;

        return (totalValue, currency);
    }

    public async Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>?> GetConversionTrendsAsync(Guid websiteId, string timeframe, string? goalName = null)
    {
        if (websiteId == Guid.Empty) return null;

        // Validiere das Zeitformat
        timeframe = timeframe.ToLower();
        if (timeframe != "day" && timeframe != "week")
        {
            _logger.LogWarning($"Invalid timeframe: {timeframe}. Using 'day' as default.");
            timeframe = "day";
        }

        try
        {
            var trends = await _conversionRepository.GetConversionTrendsAsync(websiteId, timeframe, goalName);
            return trends;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conversion trends for website {WebsiteId}", websiteId);
            return null;
        }
    }
    
}