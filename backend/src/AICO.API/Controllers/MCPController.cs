using Microsoft.AspNetCore.Mvc;
using AICO.Application.DTOs;
using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.Reports.Executive;
using AICO.Domain.DTOs.Reports.Analytics;
using AICO.Domain.DTOs.ValidationResults;
using System.ComponentModel.DataAnnotations;

namespace AICO.API.Controllers
{
    /// <summary>
    /// MCP (Maximum Customer Profit) API Controller
    /// Core MVP functionality for profit optimization calculations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MCPController : ControllerBase
    {
        private readonly IProfitTrackingService _profitTrackingService;
        private readonly IMCPAnalyticsService _mcpAnalyticsService;
        private readonly IMCPValidationService _mcpValidationService;
        private readonly IMCPReportingService _mcpReportingService;
        private readonly ILogger<MCPController> _logger;

        public MCPController(
            IProfitTrackingService profitTrackingService,
            IMCPAnalyticsService mcpAnalyticsService,
            IMCPValidationService mcpValidationService,
            IMCPReportingService mcpReportingService,
            ILogger<MCPController> logger)
        {
            _profitTrackingService = profitTrackingService;
            _mcpAnalyticsService = mcpAnalyticsService;
            _mcpValidationService = mcpValidationService;
            _mcpReportingService = mcpReportingService;
            _logger = logger;
        }

        /// <summary>
        /// Calculate MCP between control and variant
        /// Core MVP feature: Single variant MCP calculation
        /// </summary>
        /// <param name="request">MCP calculation request</param>
        /// <returns>MCP percentage</returns>
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(MCPCalculationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<MCPCalculationResponse> CalculateMCP([FromBody] MCPCalculationRequest request)
        {
            try
            {
                _logger.LogInformation("Calculating MCP for control: {ControlRevenue}/{ControlConversions}, variant: {VariantRevenue}/{VariantConversions}",
                    request.ControlRevenue, request.ControlConversions, request.VariantRevenue, request.VariantConversions);

                var mcp = _profitTrackingService.CalculateMCP(
                    request.ControlRevenue,
                    request.ControlConversions,
                    request.VariantRevenue,
                    request.VariantConversions);

                var isSignificant = _profitTrackingService.CalculateMCPWithSignificance(
                    request.ControlRevenue,
                    request.ControlConversions,
                    request.VariantRevenue,
                    request.VariantConversions);

                var response = new MCPCalculationResponse
                {
                    MCP = mcp,
                    IsStatisticallySignificant = isSignificant.HasValue,
                    ControlRPC = request.ControlConversions > 0 ? request.ControlRevenue / request.ControlConversions : 0,
                    VariantRPC = request.VariantConversions > 0 ? request.VariantRevenue / request.VariantConversions : 0,
                    Currency = request.Currency ?? "USD"
                };

                _logger.LogInformation("MCP calculated successfully: {MCP}%, Significant: {IsSignificant}", mcp, isSignificant.HasValue);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid MCP calculation request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating MCP");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Calculate MCP for multiple variants
        /// Core MVP feature: Multi-variant comparison
        /// </summary>
        /// <param name="variants">List of variant data including control</param>
        /// <returns>MCP results for all variants</returns>
        [HttpPost("calculate-multiple")]
        [ProducesResponseType(typeof(List<MCPResult>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<List<MCPResult>> CalculateMultipleMCP([FromBody] List<VariantData> variants)
        {
            try
            {
                if (variants == null || !variants.Any())
                {
                    return BadRequest(new { error = "Variants list cannot be empty" });
                }

                if (!variants.Any(v => v.IsControl))
                {
                    return BadRequest(new { error = "Control variant is required" });
                }

                _logger.LogInformation("Calculating MCP for {VariantCount} variants", variants.Count);

                var results = _profitTrackingService.CalculateMCPForMultipleVariants(variants);

                _logger.LogInformation("MCP calculated for {ResultCount} variants", results.Count);
                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid multiple MCP calculation request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating multiple MCP");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Generate comprehensive revenue report with MCP analysis
        /// Core MVP feature: Profit-focused reporting
        /// </summary>
        /// <param name="abTestId">A/B test ID</param>
        /// <param name="startDate">Report start date</param>
        /// <param name="endDate">Report end date</param>
        /// <returns>Revenue report with MCP metrics</returns>
        [HttpGet("report/{abTestId}")]
        [ProducesResponseType(typeof(RevenueReport), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RevenueReport>> GenerateRevenueReport(
            [FromRoute] Guid abTestId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var start = startDate ?? DateTime.UtcNow.AddDays(-30);
                var end = endDate ?? DateTime.UtcNow;

                if (start >= end)
                {
                    return BadRequest(new { error = "Start date must be before end date" });
                }

                _logger.LogInformation("Generating revenue report for A/B test {AbTestId} from {StartDate} to {EndDate}",
                    abTestId, start, end);

                var report = await _profitTrackingService.GenerateRevenueReportAsync(abTestId, start, end);

                if (report.TotalConversions == 0)
                {
                    _logger.LogWarning("No data found for A/B test {AbTestId}", abTestId);
                    return NotFound(new { error = "No revenue data found for the specified A/B test and date range" });
                }

                _logger.LogInformation("Revenue report generated successfully for A/B test {AbTestId}: {TotalRevenue} revenue, {TotalConversions} conversions, {OverallMCP}% MCP",
                    abTestId, report.TotalRevenue, report.TotalConversions, report.OverallMCP);

                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating revenue report for A/B test {AbTestId}", abTestId);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Calculate ROI (Return on Investment) between variants
        /// Additional MVP feature: ROI comparison
        /// </summary>
        /// <param name="request">ROI calculation request</param>
        /// <returns>ROI percentage</returns>
        [HttpPost("roi")]
        [ProducesResponseType(typeof(ROICalculationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<ROICalculationResponse> CalculateROI([FromBody] MCPCalculationRequest request)
        {
            try
            {
                _logger.LogInformation("Calculating ROI for control: {ControlRevenue}/{ControlConversions}, variant: {VariantRevenue}/{VariantConversions}",
                    request.ControlRevenue, request.ControlConversions, request.VariantRevenue, request.VariantConversions);

                var roi = _profitTrackingService.CalculateROI(
                    request.ControlRevenue,
                    request.ControlConversions,
                    request.VariantRevenue,
                    request.VariantConversions);

                var response = new ROICalculationResponse
                {
                    ROI = roi,
                    ControlRPC = request.ControlConversions > 0 ? request.ControlRevenue / request.ControlConversions : 0,
                    VariantRPC = request.VariantConversions > 0 ? request.VariantRevenue / request.VariantConversions : 0,
                    Currency = request.Currency ?? "USD"
                };

                _logger.LogInformation("ROI calculated successfully: {ROI}%", roi);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid ROI calculation request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating ROI");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Calculate comprehensive MCP with advanced analytics
        /// Enhanced MVP feature: Advanced MCP calculation with validation
        /// </summary>
        /// <param name="request">Comprehensive MCP calculation request</param>
        /// <returns>Detailed MCP analysis</returns>
        [HttpPost("calculate-comprehensive")]
        [ProducesResponseType(typeof(MCPAnalysisReport), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MCPAnalysisReport>> CalculateComprehensiveMCP([FromBody] MCPCalculationRequestDto request)
        {
            try
            {
                _logger.LogInformation("Calculating comprehensive MCP for A/B test {AbTestId}", request.AbTestId);

                // Map to application DTO and validate the request
                var mappedRequest = new AICO.Application.DTOs.MCPCalculationRequest
                {
                    ControlRevenue = 0, // Will be populated from A/B test data
                    ControlConversions = 1, // Will be populated from A/B test data
                    VariantRevenue = 0, // Will be populated from A/B test data
                    VariantConversions = 1, // Will be populated from A/B test data
                    Currency = request.Currency ?? "USD"
                };
                var validationResult = await _mcpValidationService.ValidateComprehensiveMCPRequestAsync(mappedRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new { error = "Validation failed", details = validationResult.CriticalErrors });
                }

                // Calculate comprehensive MCP
                var startDate = request.StartDate ?? DateTime.MinValue;
                var endDate = request.EndDate ?? DateTime.MaxValue;
                var result = await _mcpAnalyticsService.CalculateMCPWithConfidenceIntervalsAsync(
                    request.AbTestId, startDate, endDate, request.ConfidenceLevel);

                _logger.LogInformation("Comprehensive MCP calculated successfully for A/B test {AbTestId}: {MCP}%", 
                    request.AbTestId, result.MCP);
                
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid comprehensive MCP calculation request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating comprehensive MCP");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Generate executive summary report
        /// Enhanced MVP feature: Executive-level MCP reporting
        /// </summary>
        /// <param name="request">Executive summary request</param>
        /// <returns>Executive summary with key MCP insights</returns>
        [HttpPost("executive-summary")]
        [ProducesResponseType(typeof(MCPExecutiveSummary), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MCPExecutiveSummary>> GenerateExecutiveSummary([FromBody] MCPReportRequest request)
        {
            try
            {
                _logger.LogInformation("Generating executive summary for A/B test {AbTestId}", request.AbTestId);

                var startDate = request.StartDate ?? DateTime.MinValue;
                var endDate = request.EndDate ?? DateTime.MaxValue;
                var summary = await _mcpReportingService.GenerateExecutiveSummaryAsync(
                    request.AbTestId, startDate, endDate);

                _logger.LogInformation("Executive summary generated successfully for A/B test {AbTestId}", request.AbTestId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating executive summary for A/B test {AbTestId}", request.AbTestId);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get real-time MCP dashboard data
        /// Enhanced MVP feature: Real-time MCP monitoring
        /// </summary>
        /// <param name="abTestId">A/B test ID</param>
        /// <returns>Real-time dashboard data</returns>
        [HttpGet("dashboard/{abTestId}")]
        [ProducesResponseType(typeof(MCPDashboardData), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MCPDashboardData>> GetDashboardData([FromRoute] Guid abTestId)
        {
            try
            {
                _logger.LogInformation("Getting dashboard data for A/B test {AbTestId}", abTestId);

                var dashboardData = await _mcpReportingService.GetRealTimeDashboardDataAsync(abTestId);

                _logger.LogInformation("Dashboard data retrieved successfully for A/B test {AbTestId}", abTestId);
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard data for A/B test {AbTestId}", abTestId);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Validate MCP calculation inputs
        /// Enhanced MVP feature: Input validation
        /// </summary>
        /// <param name="request">Validation request</param>
        /// <returns>Validation results</returns>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(ComprehensiveValidationResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ComprehensiveValidationResult>> ValidateInputs([FromBody] MCPCalculationRequestDto request)
        {
            try
            {
                _logger.LogInformation("Validating MCP inputs for A/B test {AbTestId}", request.AbTestId);

                // Map to application DTO for validation
                var mappedRequest = new AICO.Application.DTOs.MCPCalculationRequest
                {
                    ControlRevenue = 0, // Will be populated from A/B test data
                    ControlConversions = 1, // Will be populated from A/B test data
                    VariantRevenue = 0, // Will be populated from A/B test data
                    VariantConversions = 1, // Will be populated from A/B test data
                    Currency = request.Currency ?? "USD"
                };
                var validationResult = await _mcpValidationService.ValidateComprehensiveMCPRequestAsync(mappedRequest);

                _logger.LogInformation("Validation completed for A/B test {AbTestId}: {IsValid}", 
                    request.AbTestId, validationResult.IsValid);
                
                return Ok(validationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating MCP inputs");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }

    #region Request/Response DTOs



    /// <summary>
    /// Response model for MCP calculation
    /// </summary>
    public class MCPCalculationResponse
    {
        public decimal MCP { get; set; }
        public bool IsStatisticallySignificant { get; set; }
        public decimal ControlRPC { get; set; }
        public decimal VariantRPC { get; set; }
        public string Currency { get; set; } = "USD";
    }

    /// <summary>
    /// Response model for ROI calculation
    /// </summary>
    public class ROICalculationResponse
    {
        public decimal ROI { get; set; }
        public decimal ControlRPC { get; set; }
        public decimal VariantRPC { get; set; }
        public string Currency { get; set; } = "USD";
    }

    /// <summary>
    /// Request model for comprehensive MCP calculation
    /// </summary>
    public class MCPCalculationRequestDto
    {
        [Required]
        public Guid AbTestId { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Range(0.01, 0.99, ErrorMessage = "Confidence level must be between 0.01 and 0.99")]
        public double ConfidenceLevel { get; set; } = 0.95;
        
        [MaxLength(3)]
        public string? Currency { get; set; } = "USD";
    }

    /// <summary>
    /// Request model for MCP reports
    /// </summary>
    public class MCPReportRequest
    {
        [Required]
        public Guid AbTestId { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [MaxLength(3)]
        public string? Currency { get; set; } = "USD";
    }

    #endregion
}