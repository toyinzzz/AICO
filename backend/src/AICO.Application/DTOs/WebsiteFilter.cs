using System;

namespace AICO.Application.DTOs
{
    public class WebsiteFilter
    {
        public Guid? UserId { get; set; }
        public string? UrlContains { get; set; }
        public string? NameContains { get; set; }
        public string? Industry { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? LastAnalyzedAfter { get; set; }
        public DateTime? LastAnalyzedBefore { get; set; }
    }
}