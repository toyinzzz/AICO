using System;
using AICO.Domain.ValueObjects;

namespace AICO.Domain.Events
{
    /// <summary>
    /// Domain event raised when a conversion is recorded
    /// </summary>
    public class ConversionRecorded : DomainEvent
    {
        /// <summary>
        /// The ID of the conversion
        /// </summary>
        public Guid ConversionId { get; }
        
        /// <summary>
        /// The ID of the website
        /// </summary>
        public Guid WebsiteId { get; }
        
        /// <summary>
        /// The type of conversion
        /// </summary>
        public ConversionType ConversionType { get; }
        
        /// <summary>
        /// The name of the goal
        /// </summary>
        public string GoalName { get; }
        
        /// <summary>
        /// The value of the conversion (if applicable)
        /// </summary>
        public decimal? Value { get; }
        
        /// <summary>
        /// The ID of the session (if available)
        /// </summary>
        public Guid? SessionId { get; }
        
        public ConversionRecorded(Guid conversionId, Guid websiteId, ConversionType conversionType, string goalName, decimal? value = null, Guid? sessionId = null)
        {
            ConversionId = conversionId;
            WebsiteId = websiteId;
            ConversionType = conversionType;
            GoalName = goalName;
            Value = value;
            SessionId = sessionId;
        }
    }
}