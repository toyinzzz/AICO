using System.Threading.Tasks;

namespace AICO.Application.Interfaces.ExternalServices
{
    public interface IAIService
    {
        Task<string> GenerateVariantContentAsync(AIVariantRequest request);
        Task<AIAnalysisResult> AnalyzeContentPerformanceAsync(string content, string metrics);
        Task<string> OptimizeContentAsync(string originalContent, string optimizationGoal);
        Task<AIRecommendation[]> GetContentRecommendationsAsync(string content, string targetAudience);
    }

    public record AIVariantRequest(
        string OriginalContent,
        string OptimizationGoal,
        string TargetAudience,
        string ContentType,
        string[] Keywords
    );

    public record AIAnalysisResult(
        double PerformanceScore,
        string[] Strengths,
        string[] Weaknesses,
        string[] Recommendations
    );

    public record AIRecommendation(
        string Type,
        string Description,
        double ConfidenceScore,
        string ExpectedImpact
    );
}