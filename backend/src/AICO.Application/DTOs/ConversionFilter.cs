using System;
using AICO.Domain.ValueObjects; // For ConversionType

namespace AICO.Application.DTOs
{
    public class ConversionFilter
    {
        public Guid? WebsiteId { get; set; }
        public Guid? SessionId { get; set; }
        public ConversionType? ConversionType { get; set; }
        public string? GoalName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}