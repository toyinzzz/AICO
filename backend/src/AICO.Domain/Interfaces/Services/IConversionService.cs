using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for managing conversions
    /// </summary>
    public interface IConversionService
    {
        /// <summary>
        /// Records a new conversion
        /// </summary>
        Task<Conversion> RecordConversionAsync(Guid websiteId, string conversionType, string goalName, decimal? value = null, string currency = null, string conversionData = null, Guid? sessionId = null);
        
        /// <summary>
        /// Gets conversions for a website
        /// </summary>
        Task<IEnumerable<Conversion>> GetWebsiteConversionsAsync(Guid websiteId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets conversions for a session
        /// </summary>
        Task<IEnumerable<Conversion>> GetSessionConversionsAsync(Guid sessionId);
        
        /// <summary>
        /// Gets conversions by type
        /// </summary>
        Task<IEnumerable<Conversion>> GetConversionsByTypeAsync(Guid websiteId, string conversionType, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets conversions by goal
        /// </summary>
        Task<IEnumerable<Conversion>> GetConversionsByGoalAsync(Guid websiteId, string goalName, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Calculates conversion rate for a website
        /// </summary>
        Task<double> CalculateConversionRateAsync(Guid websiteId, string goalName = null, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Calculates total conversion value for a website
        /// </summary>
        Task<(decimal TotalValue, string Currency)> CalculateTotalValueAsync(Guid websiteId, string conversionType = null, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets conversion trends over time
        /// </summary>
        Task<IEnumerable<(DateTime Date, int Count, decimal? Value)>> GetConversionTrendsAsync(Guid websiteId, string timeframe, string goalName = null);
    }
}