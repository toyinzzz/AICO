using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Interfaces.Data;
using AICO.Domain.ValueObjects;
using AICO.Shared.Interfaces;
using MathNet.Numerics.Distributions;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AICO.Domain.Services
{
    public class AbTestService : IAbTestService
    {
        private readonly IAbTestRepository _abTestRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly IMapper<AbTestVariant, Variant> _abTestVariantMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AbTestService> _logger;

        public AbTestService(
            IAbTestRepository abTestRepository,
            IVariantRepository variantRepository,
            ILogger<AbTestService> logger,
            IMapper<AbTestVariant, Variant> abTestVariantMapper,
            IUnitOfWork unitOfWork
            )
        {
            _abTestRepository = abTestRepository;
            _variantRepository = variantRepository;
            _logger = logger;
            _abTestVariantMapper = abTestVariantMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<AbTest?> CreateTestAsync(Guid campaignId, List<AbTestVariant> abTestVariants, int trafficSplit = 50)
        {
            if (campaignId == Guid.Empty || abTestVariants == null || !abTestVariants.Any())
            {
                _logger.LogWarning("Invalid input for creating A/B test.");
                return null;
            }

            // Validate that there is exactly one control variant
            if (abTestVariants.Count(v => v.IsControl) != 1)
            {
                _logger.LogWarning("An A/B test must have exactly one control variant.");
                return null;
            }

            // Validate that the traffic split adds up to 100%
            if (abTestVariants.Sum(v => v.TrafficSplitPercentage) != 100)
            {
                _logger.LogWarning("The sum of traffic splits for all variants must be 100.");
                return null;
            }

            // TestType darf nicht null sein, daher Standardwert verwenden
            var abTest = AbTest.Create(
                name: $"A/B Test for Campaign {campaignId}",
                description: null,
                campaignId: campaignId,
                testType: TestType.Content, // Default auf Content-Test
                targetSelector: string.Empty,
                originalContent: string.Empty,
                primaryMetric: string.Empty
            );

            foreach (var abTestVariant in abTestVariants)
            {
                abTest.AddVariant(abTestVariant);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createdTest = await _abTestRepository.AddAsync(abTest);

                var variants = abTestVariants.Select(abTestVariant =>
                {
                    var mappedVariant = _abTestVariantMapper.Map(abTestVariant);
                   
                    var variant = new Variant(
                        mappedVariant.Name,
                        createdTest.Id,
                        mappedVariant.Content,
                        mappedVariant.TrafficAllocation,
                        mappedVariant.IsControl,
                        mappedVariant.AiPrompt,
                        mappedVariant.AiConfidenceScore
                    );
                    return variant;
                }).ToList();

                foreach (var variant in variants)
                {
                    await _variantRepository.AddAsync(variant);
                }

                await _unitOfWork.CommitTransactionAsync();

                return createdTest;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error creating A/B test.");
                return null;
            }
        }

        public async Task<AbTestVariant?> GetVariantForVisitorAsync(Guid testId, string visitorId, string? userAgent = null)
        {
            var test = await _abTestRepository.GetByIdAsync(testId);
            if (test == null || test.Status != AbTestStatus.Running)
            {
                return null;
            }

            var variants = test.Variants?.ToList() ?? new List<AbTestVariant>();
            if (!variants.Any())
            {
                return null;
            }

            int visitorHash = Math.Abs(visitorId.GetHashCode());
            int bucket = visitorHash % 100; // Value from 0 to 99

            int cumulativePercentage = 0;
            foreach (var variant in variants.OrderBy(v => v.Id)) // Order for consistent assignment
            {
                cumulativePercentage += (int)variant.TrafficSplitPercentage;
                if (bucket < cumulativePercentage)
                {
                    return variant;
                }
            }

            // Fallback in case of rounding errors or misconfiguration
            return variants.LastOrDefault();
        }

        public async Task RecordVariantViewAsync(Guid variantId, string visitorId, string? sessionId = null)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant != null)
            {
                variant.RecordView();
                await _variantRepository.UpdateAsync(variant);
            }
        }

        public async Task RecordVariantConversionAsync(Guid variantId, string visitorId, decimal? value = null, string? conversionType = null)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant != null)
            {
                variant.RecordConversion();
                await _variantRepository.UpdateAsync(variant);
            }
        }

        public async Task<AbTestResults?> GetTestResultsAsync(Guid testId)
        {
            var test = await _abTestRepository.GetByIdAsync(testId);
            if (test == null)
            {
                _logger.LogWarning("Test with ID {TestId} not found.", testId);
                return null;
            }

            var variants = test.Variants?.ToList() ?? new List<AbTestVariant>();
            if (!variants.Any())
            {
                _logger.LogError("Test with ID {TestId} has no variants to analyze.", testId);
                return null;
            }

            var controlVariant = variants.FirstOrDefault(v => v.IsControl) ?? variants.First();
            var testVariants = variants.Where(v => v.Id != controlVariant.Id).ToList();

            var controlResults = MapToVariantResults(controlVariant, controlVariant);
            var testVariantResults = testVariants.Select(v => MapToVariantResults(v, controlVariant)).ToList();

            var bestTestVariant = testVariantResults.OrderByDescending(v => v.ConversionRate).FirstOrDefault();
            bool isSignificant = false;
            decimal pValue = 1;

            if (bestTestVariant != null)
            {
                (isSignificant, pValue) = CalculateStatisticalSignificance(controlResults, bestTestVariant);
            }

            var totalSampleSize = variants.Sum(v => (long)v.Views);
            var winningVariant = isSignificant ? bestTestVariant : null;

            return new AbTestResults
            {
                TestId = test.Id,
                TestName = test.Name,
                Status = test.Status.ToString(),
                StartDate = test.StartedAt ?? DateTime.UtcNow,
                EndDate = test.EndedAt,
                ControlVariant = controlResults,
                TestVariants = testVariantResults,
                IsSignificant = isSignificant,
                PValue = pValue,
                WinningVariantId = winningVariant?.VariantId,
                ImprovementPercentage = winningVariant?.ImprovementPercentage ?? 0,
                TotalSampleSize = (int)totalSampleSize,
                DurationDays = (int)((test.EndedAt ?? DateTime.UtcNow) - (test.StartedAt ?? DateTime.UtcNow)).TotalDays,
                CalculatedAt = DateTime.UtcNow
            };
        }

        public async Task<bool> IsTestStatisticallySignificantAsync(Guid testId, double confidenceLevel = 0.95)
        {
            var results = await GetTestResultsAsync(testId);
            if (results == null) return false;

            return results.PValue < (decimal)(1 - confidenceLevel);
        }

        public async Task<double> CalculateConversionRateAsync(Guid variantId)
        {
            var variant = await _variantRepository.GetByIdAsync(variantId);
            if (variant == null)
            {
                return 0;
            }

            return variant.GetConversionRate();
        }

        public async Task<AbTestVariant?> DeclareWinnerAsync(Guid testId, Guid? winningVariantId = null)
        {
            var test = await _abTestRepository.GetByIdAsync(testId);
            if (test == null)
            {
                return null;
            }

            test.Stop();
            await _abTestRepository.UpdateAsync(test);

            if (winningVariantId.HasValue)
            {
                var variant = test.Variants?.FirstOrDefault(v => v.Id == winningVariantId.Value);
                return variant;
            }

            return null;
        }

        public async Task<IEnumerable<TestPerformancePoint>?> GetTestPerformanceHistoryAsync(Guid testId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var test = await _abTestRepository.GetByIdAsync(testId);
            if (test == null || test.Variants == null || !test.Variants.Any())
            {
                _logger.LogWarning("Test with ID {TestId} not found or has no variants.", testId);
                return null;
            }

            var control = test.Variants.FirstOrDefault(x => x.IsControl);
            if (control == null || control.Views == 0)
            {
                _logger.LogWarning("No valid control variant or control has zero views.");
                return null;
            }

            var controlConversionRate = control.Views > 0 ? (double)control.Conversions / control.Views : 0;

            var points = new List<TestPerformancePoint>();
            foreach (var v in test.Variants)
            {
                if (v.Views == 0) continue; // Skip variants with no views to avoid division by zero

                var conversionRate = (double)v.Conversions / v.Views;
                decimal improvement = 0;
                if (!v.IsControl && controlConversionRate > 0)
                {
                    improvement = (decimal)((conversionRate - controlConversionRate) / controlConversionRate * 100);
                }

                (decimal ciLower, decimal ciUpper) = CalculateConfidenceInterval((int)v.Conversions, (int)v.Views);

                decimal statisticalSignificance = 1; // Default-Wert

                if (!v.IsControl && control.Views > 0 && v.Views > 0)
                {
                    var controlResults = new VariantResults
                    {
                        Visitors = (int)control.Views,
                        Conversions = (int)control.Conversions
                    };
                    var variantResults = new VariantResults
                    {
                        Visitors = (int)v.Views,
                        Conversions = (int)v.Conversions
                    };
                    (_, statisticalSignificance) = CalculateStatisticalSignificance(controlResults, variantResults);
                }

                points.Add(new TestPerformancePoint
                {
                    Date = test.StartedAt ?? DateTime.UtcNow,
                    ControlVisitors = v.IsControl ? (int)v.Views : 0,
                    ControlConversions = v.IsControl ? (int)v.Conversions : 0,
                    ControlConversionRate = (decimal)(v.IsControl ? conversionRate : 0),
                    TestVisitors = !v.IsControl ? (int)v.Views : 0,
                    TestConversions = !v.IsControl ? (int)v.Conversions : 0,
                    TestConversionRate = (decimal)(!v.IsControl ? conversionRate : 0),
                    ImprovementPercentage = v.IsControl ? 0 : improvement,
                    StatisticalSignificance = statisticalSignificance,
                    ConfidenceIntervalLower = ciLower,
                    ConfidenceIntervalUpper = ciUpper
                });
            }

            return points;
        }

        public Task<int> CalculateRequiredSampleSizeAsync(double baselineConversionRate, double minimumDetectableEffect, double power = 0.8, double significance = 0.05)
        {
            if (baselineConversionRate <= 0 || baselineConversionRate >= 1)
                throw new ArgumentOutOfRangeException(nameof(baselineConversionRate), "Baseline conversion rate must be between 0 and 1.");
            if (minimumDetectableEffect <= 0)
                throw new ArgumentOutOfRangeException(nameof(minimumDetectableEffect), "Minimum detectable effect must be positive.");

            double p1 = baselineConversionRate;
            double p2 = baselineConversionRate + minimumDetectableEffect;
            if (p2 >= 1)
                throw new ArgumentOutOfRangeException(nameof(minimumDetectableEffect), "Minimum detectable effect is too large, resulting in a conversion rate >= 1.");

            double zAlpha = Normal.InvCDF(0, 1, 1 - (significance / 2));
            double zBeta = Normal.InvCDF(0, 1, power);

            double var1 = p1 * (1 - p1);
            double var2 = p2 * (1 - p2);

            double numerator = Math.Pow(zAlpha + zBeta, 2) * (var1 + var2);
            double denominator = Math.Pow(p2 - p1, 2);

            if (denominator == 0) return Task.FromResult(int.MaxValue);

            int sampleSizePerGroup = (int)Math.Ceiling(numerator / denominator);

            return Task.FromResult(sampleSizePerGroup);
        }

        private double CalculateConversionRate(int views, int conversions)
        {
            if (views == 0)
            {
                return 0;
            }

            return (double)conversions / views;
        }

        private (bool, decimal) CalculateStatisticalSignificance(VariantResults control, VariantResults variant, double confidenceLevel = 0.95)
        {
            if (control.Visitors == 0 || variant.Visitors == 0)
            {
                return (false, 1);
            }

            double p1 = (double)control.Conversions / control.Visitors;
            double p2 = (double)variant.Conversions / variant.Visitors;

            double pPooled = (double)(control.Conversions + variant.Conversions) / (control.Visitors + variant.Visitors);
            double se = Math.Sqrt(pPooled * (1 - pPooled) * (1.0 / control.Visitors + 1.0 / variant.Visitors));

            if (se == 0)
            {
                return (p1 == p2, p1 == p2 ? 0 : 1);
            }

            double z = (p2 - p1) / se;
            double pValue = 2 * (1 - Normal.CDF(0, 1, Math.Abs(z)));

            return (pValue < (1 - confidenceLevel), (decimal)pValue);
        }

        private VariantResults MapToVariantResults(AbTestVariant variant, AbTestVariant control)
        {
            int visitors = (int)variant.Views;
            int conversions = (int)variant.Conversions;
            decimal conversionRate = (visitors > 0) ? (decimal)conversions / visitors : 0;

            decimal improvement = 0;
            if (variant.Id != control.Id)
            {
                decimal controlConversionRate = (control.Views > 0) ? (decimal)control.Conversions / control.Views : 0;
                if (controlConversionRate > 0)
                {
                    improvement = (conversionRate - controlConversionRate) / controlConversionRate;
                }
            }

            (decimal lower, decimal upper) = CalculateConfidenceInterval(conversions, visitors);

            return new VariantResults
            {
                VariantId = variant.Id,
                Name = variant.Name,
                Visitors = visitors,
                Conversions = conversions,
                ConversionRate = conversionRate,
                IsControl = variant.Id == control.Id,
                ImprovementPercentage = (variant.Id != control.Id) ? improvement * 100 : (decimal?)null,
                ConfidenceIntervalLower = lower,
                ConfidenceIntervalUpper = upper,
            };
        }

        private (decimal, decimal) CalculateConfidenceInterval(int conversions, int visitors, double confidenceLevel = 0.95)
        {
            if (visitors == 0) return (0, 0);

            double p = (double)conversions / visitors;
            double z = Normal.InvCDF(0, 1, 1 - (1 - confidenceLevel) / 2);
            double z2 = z * z;

            double center = (p + z2 / (2 * visitors)) / (1 + z2 / visitors);
            double error_margin = (z / (1 + z2 / visitors)) * Math.Sqrt(p * (1 - p) / visitors + z2 / (4 * visitors * visitors));

            decimal lower = (decimal)(center - error_margin);
            decimal upper = (decimal)(center + error_margin);

            return (lower < 0 ? 0 : lower, upper > 1 ? 1 : upper);
        }
    }
    

    
}
