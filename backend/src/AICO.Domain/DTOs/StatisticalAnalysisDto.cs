namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Result of statistical significance calculation
    /// </summary>
    public class StatisticalSignificanceResult
    {
        public bool IsSignificant { get; set; }
        public double PValue { get; set; }
        public double ConfidenceLevel { get; set; }
        public double ZScore { get; set; }
        public double ControlConversionRate { get; set; }
        public double VariantConversionRate { get; set; }
        public double Improvement { get; set; }
        public double ImprovementPercentage { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Confidence interval for a statistical measure
    /// </summary>
    public class ConfidenceInterval
    {
        public double Lower { get; set; }
        public double Upper { get; set; }
        public double ConfidenceLevel { get; set; }
    }
}