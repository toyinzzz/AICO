namespace AICO.Domain.DTOs
{
    /// <summary>
    /// Prediction of variant performance before testing
    /// </summary>
    public class VariantPerformancePrediction
    {
        /// <summary>
        /// Variant ID being predicted
        /// </summary>
        public Guid VariantId { get; set; }

        /// <summary>
        /// Predicted conversion rate improvement (percentage)
        /// </summary>
        public double PredictedConversionImprovement { get; set; }

        /// <summary>
        /// Predicted conversion rate (percentage)
        /// </summary>
        public double PredictedConversionRate { get; set; }

        /// <summary>
        /// Confidence level of the prediction (0-1)
        /// </summary>
        public double ConfidenceLevel { get; set; }

        /// <summary>
        /// Predicted click-through rate
        /// </summary>
        public double PredictedClickThroughRate { get; set; }

        /// <summary>
        /// Predicted engagement score
        /// </summary>
        public double PredictedEngagementScore { get; set; }

        /// <summary>
        /// Risk assessment (Low, Medium, High)
        /// </summary>
        public string RiskAssessment { get; set; } = string.Empty;

        /// <summary>
        /// Key factors influencing the prediction
        /// </summary>
        public List<string> InfluencingFactors { get; set; } = new();

        /// <summary>
        /// Recommended test duration in days
        /// </summary>
        public int RecommendedTestDuration { get; set; }

        /// <summary>
        /// Minimum sample size for statistical significance
        /// </summary>
        public int MinimumSampleSize { get; set; }

        /// <summary>
        /// Performance prediction by audience segment
        /// </summary>
        public Dictionary<string, double> SegmentPredictions { get; set; } = new();

        /// <summary>
        /// Model used for prediction
        /// </summary>
        public string PredictionModel { get; set; } = string.Empty;

        /// <summary>
        /// Prediction timestamp
        /// </summary>
        public DateTime PredictedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Additional prediction metadata
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}