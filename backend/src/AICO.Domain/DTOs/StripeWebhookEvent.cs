namespace AICO.Domain.DTOs
{
    public class StripeWebhookEvent
    {
        public string Type { get; set; } = string.Empty;
        public StripePaymentData Data { get; set; } = new StripePaymentData();
    }
    
    public class StripePaymentData
    {
        public string Id { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
        
    }
}