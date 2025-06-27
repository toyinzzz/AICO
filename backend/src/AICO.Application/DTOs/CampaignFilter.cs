using System;
using AICO.Domain.Entities; // For CampaignStatus

namespace AICO.Application.DTOs
{
    public class CampaignFilter
    {
        public Guid? WebsiteId { get; set; }
        public Guid? UserId { get; set; }
        public CampaignStatus? Status { get; set; }
        public string? NameContains { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
    }
}