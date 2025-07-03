using System;
using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Benchmark analysis comparing performance against industry standards
/// </summary>
public class MCPBenchmarkAnalysis
{
    public decimal IndustryAverageMCP { get; set; }
    public decimal PortfolioAverageMCP { get; set; }
    public string PerformanceRating { get; set; } = string.Empty;
    public decimal BenchmarkScore { get; set; }
    public List<string> StrengthAreas { get; set; } = new();
    public List<string> ImprovementAreas { get; set; } = new();
    public string BenchmarkType { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public DateTime BenchmarkDate { get; set; }
    public List<string> Recommendations { get; set; } = new();
}