namespace AICO.Domain.Interfaces.ExternalServices
{
    public interface IAIService
    {
        Task<List<string>> GenerateContentVariationsAsync(string originalContent, int numberOfVariations);
        Task<string> OptimizeContentAsync(string content, string targetAudience);
        Task<Dictionary<string, object>> AnalyzeContentPerformanceAsync(string content, Dictionary<string, object> metrics);
        Task<string> AnalyzePageAsync(string url);
        Task<List<string>> GenerateCopyAsync(string prompt, int count);
        Task<List<string>> GenerateHeadlinesAsync(string prompt, int count);
        Task<List<string>> GenerateCtasAsync(string prompt, int count);
    }
}