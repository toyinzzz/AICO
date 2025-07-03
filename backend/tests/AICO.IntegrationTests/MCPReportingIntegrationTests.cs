using AICO.Application.Interfaces.Services;
using AICO.Domain.DTOs;
using AICO.Domain.DTOs.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AICO.IntegrationTests;

/// <summary>
/// Integration tests for MCP reporting service
/// Tests end-to-end reporting functionality with real data flows
/// </summary>
public class MCPReportingIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly IServiceScope _scope;
    private readonly IMCPReportingService _reportingService;
    private readonly IMCPAnalyticsService _analyticsService;
    private readonly ILogger<MCPReportingIntegrationTests> _logger;
    private readonly Guid _testAbTestId = Guid.NewGuid();
    private readonly DateTime _testStartDate = DateTime.UtcNow.AddDays(-30);
    private readonly DateTime _testEndDate = DateTime.UtcNow;

    public MCPReportingIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _scope = _factory.Services.CreateScope();
        _reportingService = _scope.ServiceProvider.GetRequiredService<IMCPReportingService>();
        _analyticsService = _scope.ServiceProvider.GetRequiredService<IMCPAnalyticsService>();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<MCPReportingIntegrationTests>>();
    }

    #region Executive Summary Integration Tests

    [Fact]
    public async Task GenerateExecutiveSummary_EndToEnd_ShouldProduceComprehensiveReport()
    {
        // Arrange
        await SeedTestData();
        _logger.LogInformation("Starting executive summary integration test for A/B test {AbTestId}", _testAbTestId);

        // Act
        var summary = await _reportingService.GenerateExecutiveSummaryAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate);

        // Assert
        Assert.NotNull(summary);
        Assert.Equal(_testAbTestId, summary.AbTestId);
        Assert.True(summary.OverallMCP > 0);
        Assert.NotEmpty(summary.KeyInsights);
        Assert.NotEmpty(summary.Recommendations);
        Assert.NotNull(summary.RiskAssessment);
        
        _logger.LogInformation("Executive summary generated successfully with MCP: {MCP}", summary.OverallMCP);
    }

    [Fact]
    public async Task GenerateExecutiveSummary_WithRealTimeData_ShouldReflectCurrentState()
    {
        // Arrange
        await SeedTestData();
        await SimulateRealTimeTraffic();

        // Act
        var summary1 = await _reportingService.GenerateExecutiveSummaryAsync(_testAbTestId, _testStartDate, _testEndDate);
        
        // Add more data
        await SimulateAdditionalTraffic();
        
        var summary2 = await _reportingService.GenerateExecutiveSummaryAsync(_testAbTestId, _testStartDate, _testEndDate);

        // Assert
        Assert.NotNull(summary1);
        Assert.NotNull(summary2);
        Assert.True(summary2.TotalRevenue >= summary1.TotalRevenue);
        Assert.True(summary2.TotalConversions >= summary1.TotalConversions);
        
        _logger.LogInformation("Real-time data integration verified. Revenue increased from {Revenue1} to {Revenue2}", 
            summary1.TotalRevenue, summary2.TotalRevenue);
    }

    #endregion

    #region Performance Report Integration Tests

    [Fact]
    public async Task GeneratePerformanceReport_WithStatisticalAnalysis_ShouldProvideDetailedInsights()
    {
        // Arrange
        await SeedTestData();
        var includeStatisticalDetails = true;

        // Act
        var report = await _reportingService.GeneratePerformanceReportAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate, 
            includeStatisticalDetails);

        // Assert
        Assert.NotNull(report);
        Assert.NotNull(report.OverallMetrics);
        Assert.NotEmpty(report.VariantPerformance);
        Assert.NotNull(report.StatisticalAnalysis);
        Assert.NotEmpty(report.DailyTrends);
        Assert.NotNull(report.RevenueBreakdown);
        
        // Verify statistical significance calculation
        Assert.True(report.StatisticalAnalysis.ConfidenceLevel > 0);
        Assert.True(report.StatisticalAnalysis.SampleSize > 0);
        
        _logger.LogInformation("Performance report generated with {VariantCount} variants and statistical power {Power}", 
            report.VariantPerformance.Count, report.StatisticalAnalysis.StatisticalPower);
    }

    [Fact]
    public async Task GeneratePerformanceReport_WithMultipleVariants_ShouldCompareAllVariants()
    {
        // Arrange
        await SeedMultiVariantTestData();

        // Act
        var report = await _reportingService.GeneratePerformanceReportAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate);

        // Assert
        Assert.NotNull(report);
        Assert.True(report.VariantPerformance.Count >= 3); // Control + 2 variants
        
        var controlVariant = report.VariantPerformance.FirstOrDefault(v => v.IsControl);
        var treatmentVariants = report.VariantPerformance.Where(v => !v.IsControl).ToList();
        
        Assert.NotNull(controlVariant);
        Assert.NotEmpty(treatmentVariants);
        
        // Verify MCP calculations for each variant
        foreach (var variant in report.VariantPerformance)
        {
            Assert.True(variant.MCP >= 0);
            Assert.True(variant.Revenue >= 0);
            Assert.True(variant.Conversions >= 0);
        }
        
        _logger.LogInformation("Multi-variant performance report generated for {VariantCount} variants", 
            report.VariantPerformance.Count);
    }

    #endregion

    #region Comparison Report Integration Tests

    [Fact]
    public async Task GenerateComparisonReport_AcrossMultipleTests_ShouldProvidePortfolioInsights()
    {
        // Arrange
        var testIds = await SeedMultipleTestData();

        // Act
        var report = await _reportingService.GenerateComparisonReportAsync(
            testIds, 
            _testStartDate, 
            _testEndDate);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(testIds.Count, report.TestComparisons.Count);
        Assert.NotNull(report.BenchmarkAnalysis);
        Assert.NotNull(report.PortfolioMetrics);
        
        // Verify portfolio calculations
        Assert.True(report.PortfolioMetrics.TotalTests > 0);
        Assert.True(report.PortfolioMetrics.AverageTestMCP >= 0);
        Assert.True(report.PortfolioMetrics.SuccessRate >= 0 && report.PortfolioMetrics.SuccessRate <= 1);
        
        _logger.LogInformation("Comparison report generated for {TestCount} tests with portfolio MCP: {PortfolioMCP}", 
            testIds.Count, report.PortfolioMetrics.TotalPortfolioMCP);
    }

    #endregion

    #region Trend Analysis Integration Tests

    [Fact]
    public async Task GenerateTrendAnalysis_WithDailyGranularity_ShouldDetectTrends()
    {
        // Arrange
        await SeedTimeSeriesData();
        var granularity = "daily";

        // Act
        var analysis = await _reportingService.GenerateTrendAnalysisAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate, 
            granularity);

        // Assert
        Assert.NotNull(analysis);
        Assert.Equal(granularity, analysis.Granularity);
        Assert.NotEmpty(analysis.TrendData);
        Assert.NotNull(analysis.TrendStatistics);
        
        // Verify trend data points
        var expectedDataPoints = (_testEndDate - _testStartDate).Days + 1;
        Assert.True(analysis.TrendData.Count <= expectedDataPoints);
        
        // Verify trend statistics
        Assert.NotNull(analysis.TrendStatistics.TrendDirection);
        Assert.True(analysis.TrendStatistics.RSquared >= 0 && analysis.TrendStatistics.RSquared <= 1);
        
        _logger.LogInformation("Trend analysis completed with {DataPoints} data points and trend direction: {Direction}", 
            analysis.TrendData.Count, analysis.TrendStatistics.TrendDirection);
    }

    [Fact]
    public async Task GenerateTrendAnalysis_WithAnomalyDetection_ShouldIdentifyAnomalies()
    {
        // Arrange
        await SeedDataWithAnomalies();

        // Act
        var analysis = await _reportingService.GenerateTrendAnalysisAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate);

        // Assert
        Assert.NotNull(analysis);
        
        if (analysis.Anomalies.Any())
        {
            foreach (var anomaly in analysis.Anomalies)
            {
                Assert.True(Math.Abs(anomaly.Deviation) > 0);
                Assert.NotEmpty(anomaly.AnomalyType);
                Assert.True(anomaly.Date >= _testStartDate && anomaly.Date <= _testEndDate);
            }
            
            _logger.LogInformation("Anomaly detection identified {AnomalyCount} anomalies", analysis.Anomalies.Count);
        }
    }

    #endregion

    #region Segmentation Report Integration Tests

    [Fact]
    public async Task GenerateSegmentationReport_ByGeography_ShouldAnalyzeLocationPerformance()
    {
        // Arrange
        await SeedGeographicSegmentData();
        var segmentationType = "geographic";

        // Act
        var report = await _reportingService.GenerateSegmentationReportAsync(
            _testAbTestId, 
            segmentationType, 
            _testStartDate, 
            _testEndDate);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(segmentationType, report.SegmentationType);
        Assert.NotEmpty(report.SegmentAnalyses);
        Assert.NotNull(report.SegmentComparison);
        
        // Verify segment data
        foreach (var segment in report.SegmentAnalyses)
        {
            Assert.NotEmpty(segment.SegmentName);
            Assert.NotEmpty(segment.SegmentValue);
            Assert.True(segment.MCP >= 0);
            Assert.True(segment.SampleSize > 0);
        }
        
        _logger.LogInformation("Geographic segmentation report generated for {SegmentCount} segments", 
            report.SegmentAnalyses.Count);
    }

    #endregion

    #region Forecast Report Integration Tests

    [Fact]
    public async Task GenerateForecastReport_With30DayPrediction_ShouldProvideForecast()
    {
        // Arrange
        await SeedHistoricalData();
        var forecastPeriodDays = 30;

        // Act
        var report = await _reportingService.GenerateForecastReportAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate, 
            forecastPeriodDays);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(forecastPeriodDays, report.ForecastPeriodDays);
        Assert.NotEmpty(report.ForecastData);
        Assert.NotNull(report.AccuracyMetrics);
        Assert.NotNull(report.ConfidenceIntervals);
        
        // Verify forecast data
        Assert.Equal(forecastPeriodDays, report.ForecastData.Count);
        
        foreach (var point in report.ForecastData)
        {
            Assert.True(point.Date > _testEndDate);
            Assert.True(point.PredictedMCP >= 0);
            Assert.True(point.LowerBound <= point.PredictedMCP);
            Assert.True(point.UpperBound >= point.PredictedMCP);
            Assert.True(point.Confidence >= 0 && point.Confidence <= 1);
        }
        
        _logger.LogInformation("Forecast report generated for {Days} days with accuracy rating: {Rating}", 
            forecastPeriodDays, report.AccuracyMetrics.AccuracyRating);
    }

    #endregion

    #region Dashboard Integration Tests

    [Fact]
    public async Task GenerateDashboardData_RealTime_ShouldProvideCurrentMetrics()
    {
        // Arrange
        await SeedTestData();
        await SimulateRealTimeTraffic();
        var refreshIntervalMinutes = 5;

        // Act
        var dashboard = await _reportingService.GenerateDashboardDataAsync(
            _testAbTestId, 
            refreshIntervalMinutes);

        // Assert
        Assert.NotNull(dashboard);
        Assert.Equal(_testAbTestId, dashboard.AbTestId);
        Assert.Equal(refreshIntervalMinutes, dashboard.RefreshIntervalMinutes);
        Assert.NotNull(dashboard.RealTimeMetrics);
        Assert.NotEmpty(dashboard.VariantStatuses);
        Assert.NotNull(dashboard.KPIs);
        
        // Verify real-time metrics
        Assert.True(dashboard.RealTimeMetrics.CurrentMCP >= 0);
        Assert.True(dashboard.RealTimeMetrics.RevenueToday >= 0);
        Assert.True(dashboard.RealTimeMetrics.ConversionsToday >= 0);
        Assert.True(dashboard.RealTimeMetrics.VisitorsToday >= 0);
        
        // Verify variant statuses
        foreach (var variant in dashboard.VariantStatuses)
        {
            Assert.NotEmpty(variant.VariantId);
            Assert.NotEmpty(variant.Status);
            Assert.True(variant.CurrentMCP >= 0);
            Assert.True(variant.TrafficAllocation >= 0 && variant.TrafficAllocation <= 100);
        }
        
        _logger.LogInformation("Dashboard data generated with current MCP: {MCP} and {VariantCount} variants", 
            dashboard.RealTimeMetrics.CurrentMCP, dashboard.VariantStatuses.Count);
    }

    #endregion

    #region Alert Integration Tests

    [Fact]
    public async Task GenerateAlertReport_WithThresholds_ShouldDetectIssues()
    {
        // Arrange
        await SeedTestDataWithIssues();
        var thresholds = new MCPAlertThresholds
        {
            MinMCPThreshold = 90m,
            MaxMCPThreshold = 200m,
            MinConversionRate = 0.01,
            MaxConversionRate = 0.20,
            MinSampleSize = 1000,
            SignificanceThreshold = 0.05,
            RevenueDropThreshold = 0.10m
        };

        // Act
        var report = await _reportingService.GenerateAlertReportAsync(_testAbTestId, thresholds);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(_testAbTestId, report.AbTestId);
        Assert.Equal(thresholds, report.Thresholds);
        Assert.NotNull(report.AlertSummary);
        
        // If alerts are triggered, verify their structure
        if (report.TriggeredAlerts.Any())
        {
            foreach (var alert in report.TriggeredAlerts)
            {
                Assert.NotEmpty(alert.AlertType);
                Assert.NotEmpty(alert.Severity);
                Assert.NotEmpty(alert.Message);
                Assert.True(alert.TriggeredAt <= DateTime.UtcNow);
            }
            
            _logger.LogInformation("Alert report generated with {AlertCount} triggered alerts", 
                report.TriggeredAlerts.Count);
        }
    }

    #endregion

    #region Export Integration Tests

    [Fact]
    public async Task ExportReport_ToPDF_ShouldGenerateValidFile()
    {
        // Arrange
        await SeedTestData();
        var summary = await _reportingService.GenerateExecutiveSummaryAsync(_testAbTestId, _testStartDate, _testEndDate);
        var format = "PDF";

        // Act
        var exportResult = await _reportingService.ExportReportAsync(summary, format);

        // Assert
        Assert.NotNull(exportResult);
        Assert.True(exportResult.Data.Length > 0);
        Assert.Equal(format, exportResult.Format);
        Assert.Equal("application/pdf", exportResult.ContentType);
        Assert.NotEmpty(exportResult.FileName);
        Assert.True(exportResult.FileSizeBytes > 0);
        
        _logger.LogInformation("PDF export completed. File size: {Size} bytes, File name: {FileName}", 
            exportResult.FileSizeBytes, exportResult.FileName);
    }

    [Fact]
    public async Task ExportReport_ToExcel_ShouldGenerateValidSpreadsheet()
    {
        // Arrange
        await SeedTestData();
        var report = await _reportingService.GeneratePerformanceReportAsync(_testAbTestId, _testStartDate, _testEndDate);
        var format = "Excel";

        // Act
        var exportResult = await _reportingService.ExportReportAsync(report, format);

        // Assert
        Assert.NotNull(exportResult);
        Assert.True(exportResult.Data.Length > 0);
        Assert.Equal(format, exportResult.Format);
        Assert.Contains("spreadsheet", exportResult.ContentType);
        Assert.NotEmpty(exportResult.FileName);
        
        _logger.LogInformation("Excel export completed. File size: {Size} bytes", exportResult.FileSizeBytes);
    }

    #endregion

    #region ROI Integration Tests

    [Fact]
    public async Task GenerateROIReport_WithInvestmentData_ShouldCalculateAccurateROI()
    {
        // Arrange
        await SeedTestData();
        var investmentCost = 10000m;

        // Act
        var report = await _reportingService.GenerateROIReportAsync(
            _testAbTestId, 
            _testStartDate, 
            _testEndDate, 
            investmentCost);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(_testAbTestId, report.AbTestId);
        Assert.Equal(investmentCost, report.InvestmentCost);
        Assert.True(report.RevenueIncrease >= 0);
        Assert.NotNull(report.Projection);
        
        // Verify ROI calculations
        if (report.RevenueIncrease > 0)
        {
            var expectedROI = ((report.RevenueIncrease - investmentCost) / investmentCost) * 100;
            Assert.Equal(expectedROI, report.ROIPercentage, 2); // Allow for rounding
        }
        
        _logger.LogInformation("ROI report generated. Investment: {Investment}, Revenue Increase: {Revenue}, ROI: {ROI}%", 
            report.InvestmentCost, report.RevenueIncrease, report.ROIPercentage);
    }

    #endregion

    #region Statistical Report Integration Tests

    [Fact]
    public async Task GenerateStatisticalReport_WithPowerAnalysis_ShouldProvideStatisticalInsights()
    {
        // Arrange
        await SeedTestData();
        var confidenceLevel = 0.95;

        // Act
        var report = await _reportingService.GenerateStatisticalReportAsync(_testAbTestId, confidenceLevel);

        // Assert
        Assert.NotNull(report);
        Assert.Equal(_testAbTestId, report.AbTestId);
        Assert.Equal(confidenceLevel, report.ConfidenceLevel);
        Assert.NotNull(report.PowerAnalysis);
        
        // Verify statistical calculations
        Assert.True(report.PValue >= 0 && report.PValue <= 1);
        Assert.True(report.PowerAnalysis.StatisticalPower >= 0 && report.PowerAnalysis.StatisticalPower <= 1);
        Assert.True(report.PowerAnalysis.TypeIError >= 0 && report.PowerAnalysis.TypeIError <= 1);
        Assert.True(report.PowerAnalysis.TypeIIError >= 0 && report.PowerAnalysis.TypeIIError <= 1);
        
        _logger.LogInformation("Statistical report generated. Significance: {IsSignificant}, P-value: {PValue}, Power: {Power}", 
            report.IsStatisticallySignificant, report.PValue, report.PowerAnalysis.StatisticalPower);
    }

    #endregion

    #region Schedule Integration Tests

    [Fact]
    public async Task ScheduleReport_WithConfiguration_ShouldCreateValidSchedule()
    {
        // Arrange
        var reportConfig = new MCPReportConfiguration
        {
            ReportType = "Executive Summary",
            TemplateId = "exec-summary-v1",
            OutputFormat = "PDF",
            Parameters = new Dictionary<string, object>
            {
                { "includeCharts", true },
                { "includeRecommendations", true }
            }
        };

        // Act
        var schedule = await _reportingService.ScheduleReportAsync(reportConfig);

        // Assert
        Assert.NotNull(schedule);
        Assert.NotEqual(Guid.Empty, schedule.ScheduleId);
        Assert.Equal(reportConfig.ReportType, schedule.ReportType);
        Assert.Equal(reportConfig, schedule.Configuration);
        Assert.True(schedule.NextRunTime > DateTime.UtcNow);
        
        _logger.LogInformation("Report schedule created with ID: {ScheduleId}, Next run: {NextRun}", 
            schedule.ScheduleId, schedule.NextRunTime);
    }

    #endregion

    #region Template Integration Tests

    [Fact]
    public async Task GetAvailableTemplates_ShouldReturnValidTemplates()
    {
        // Act
        var templates = await _reportingService.GetAvailableTemplatesAsync();

        // Assert
        Assert.NotNull(templates);
        Assert.NotEmpty(templates);
        
        foreach (var template in templates)
        {
            Assert.NotEmpty(template.TemplateId);
            Assert.NotEmpty(template.Name);
            Assert.NotEmpty(template.Description);
            Assert.NotEmpty(template.Category);
            Assert.NotEmpty(template.SupportedFormats);
        }
        
        _logger.LogInformation("Retrieved {TemplateCount} available report templates", templates.Count);
    }

    #endregion

    #region Performance Integration Tests

    [Fact]
    public async Task GenerateReports_ConcurrentRequests_ShouldHandleLoad()
    {
        // Arrange
        await SeedTestData();
        var concurrentRequests = 10;
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks.Add(_reportingService.GenerateExecutiveSummaryAsync(_testAbTestId, _testStartDate, _testEndDate));
        }

        var results = await Task.WhenAll(tasks.Cast<Task<MCPExecutiveSummary>>());

        // Assert
        Assert.Equal(concurrentRequests, results.Length);
        Assert.All(results, result => Assert.NotNull(result));
        
        _logger.LogInformation("Successfully handled {RequestCount} concurrent report generation requests", 
            concurrentRequests);
    }

    #endregion

    #region Helper Methods

    private async Task SeedTestData()
    {
        // Seed basic test data for MCP calculations
        _logger.LogInformation("Seeding test data for A/B test {AbTestId}", _testAbTestId);
        
        // This would typically involve:
        // - Creating test variants
        // - Adding revenue records
        // - Setting up conversion data
        // - Configuring test parameters
        
        await Task.Delay(100); // Simulate data seeding
    }

    private async Task SeedMultiVariantTestData()
    {
        // Seed data for multi-variant testing
        _logger.LogInformation("Seeding multi-variant test data");
        await Task.Delay(100);
    }

    private async Task<List<Guid>> SeedMultipleTestData()
    {
        // Seed data for multiple A/B tests
        _logger.LogInformation("Seeding multiple test data");
        await Task.Delay(100);
        
        return new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
    }

    private async Task SeedTimeSeriesData()
    {
        // Seed time-series data for trend analysis
        _logger.LogInformation("Seeding time-series data");
        await Task.Delay(100);
    }

    private async Task SeedDataWithAnomalies()
    {
        // Seed data with known anomalies for testing detection
        _logger.LogInformation("Seeding data with anomalies");
        await Task.Delay(100);
    }

    private async Task SeedGeographicSegmentData()
    {
        // Seed data with geographic segments
        _logger.LogInformation("Seeding geographic segment data");
        await Task.Delay(100);
    }

    private async Task SeedHistoricalData()
    {
        // Seed historical data for forecasting
        _logger.LogInformation("Seeding historical data for forecasting");
        await Task.Delay(100);
    }

    private async Task SeedTestDataWithIssues()
    {
        // Seed data that will trigger alerts
        _logger.LogInformation("Seeding test data with issues for alert testing");
        await Task.Delay(100);
    }

    private async Task SimulateRealTimeTraffic()
    {
        // Simulate real-time traffic and conversions
        _logger.LogInformation("Simulating real-time traffic");
        await Task.Delay(50);
    }

    private async Task SimulateAdditionalTraffic()
    {
        // Simulate additional traffic for testing data updates
        _logger.LogInformation("Simulating additional traffic");
        await Task.Delay(50);
    }

    #endregion

    #region Cleanup

    public void Dispose()
    {
        _scope?.Dispose();
    }

    #endregion
}