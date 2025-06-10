using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for AnalysisResult entity operations
    /// </summary>
    public interface IAnalysisResultRepository : IRepository<AnalysisResult>
    {
        /// <summary>
        /// Gets all analysis results for a specific website
        /// </summary>
        Task<IEnumerable<AnalysisResult>> GetByWebsiteIdAsync(Guid websiteId);
        
        /// <summary>
        /// Gets the latest analysis result for a specific website
        /// </summary>
        Task<AnalysisResult> GetLatestByWebsiteIdAsync(Guid websiteId);
        
        /// <summary>
        /// Gets analysis results by type for a specific website
        /// </summary>
        Task<IEnumerable<AnalysisResult>> GetByTypeAndWebsiteIdAsync(string analysisType, Guid websiteId);
    }
} 