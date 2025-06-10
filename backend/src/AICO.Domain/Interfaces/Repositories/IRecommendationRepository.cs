using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AICO.Domain.Entities;

namespace AICO.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Recommendation entity operations
    /// </summary>
    public interface IRecommendationRepository : IRepository<Recommendation>
    {
        /// <summary>
        /// Gets all recommendations for a specific analysis result
        /// </summary>
        Task<IEnumerable<Recommendation>> GetByAnalysisResultIdAsync(Guid analysisResultId);
        
        /// <summary>
        /// Gets recommendations by category for a specific analysis result
        /// </summary>
        Task<IEnumerable<Recommendation>> GetByCategoryAndAnalysisResultIdAsync(string category, Guid analysisResultId);
        
        /// <summary>
        /// Gets recommendations by implementation status for a specific analysis result
        /// </summary>
        Task<IEnumerable<Recommendation>> GetByImplementationStatusAndAnalysisResultIdAsync(bool isImplemented, Guid analysisResultId);
    }
} 