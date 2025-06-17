using System;

namespace AICO.Application.DTOs
{
    public class SessionFilter
    {
        public Guid? WebsiteId { get; set; }
        public Guid? UserId { get; set; }
        public string? VisitorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? HasConverted { get; set; }
        public string? EntryPage { get; set; }
        public string? IpAddress { get; set; }
    }
}