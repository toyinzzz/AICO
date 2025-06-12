namespace AICO.Domain.DTOs
{
    public class StripeWebhookEvent
    {
        public string Type { get; set; }
        public StripePaymentData Data { get; set; }
    }
    
    public class StripePaymentData
    {
        public string Id { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        
    }
}