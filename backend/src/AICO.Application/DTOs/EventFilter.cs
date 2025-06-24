using System;
using AICO.Domain.ValueObjects; // For EventType

namespace AICO.Application.DTOs
{
    public class EventFilter
    {
        public Guid? WebsiteId { get; set; }
        public Guid? SessionId { get; set; }
        public EventType? EventType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? IpAddress { get; set; }
    }
}