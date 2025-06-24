using System;

namespace AICO.Application.DTOs
{
    public class SnippetFilter
    {
        public Guid? WebsiteId { get; set; }
        public string? NameContains { get; set; }
        public string? Type { get; set; }
        public bool? IsActive { get; set; }
        public string? Version { get; set; }
    }
}