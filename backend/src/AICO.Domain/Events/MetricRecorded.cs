using System;
using AICO.Domain.ValueObjects;

namespace AICO.Domain.Events
{
    /// <summary>
    /// Domain event raised when a metric is recorded
    /// </summary>
    public class MetricRecorded : DomainEvent
    {
        /// <summary>
        /// The ID of the metric
        /// </summary>
        public Guid MetricId { get; }
        
        /// <summary>
        /// The ID of the website
        /// </summary>
        public Guid WebsiteId { get; }
        
        /// <summary>
        /// The name of the metric
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The category of the metric
        /// </summary>
        public MetricCategory Category { get; }
        
        /// <summary>
        /// The value of the metric
        /// </summary>
        public double Value { get; }
        
        public MetricRecorded(Guid metricId, Guid websiteId, string name, MetricCategory category, double value)
        {
            MetricId = metricId;
            WebsiteId = websiteId;
            Name = name;
            Category = category;
            Value = value;
        }
    }
}