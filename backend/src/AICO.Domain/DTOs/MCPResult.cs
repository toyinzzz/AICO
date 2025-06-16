namespace AICO.Domain.DTOs
{
    public class MCPResult
    {
        public Guid VariantId { get; set; }
        public decimal MCP { get; set; }
        public bool IsStatisticallySignificant { get; set; }
        public int SampleSize { get; set; }
    }
}