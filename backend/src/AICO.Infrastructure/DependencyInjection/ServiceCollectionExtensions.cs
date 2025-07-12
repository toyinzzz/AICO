using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.ExternalServices;
using AICO.Application.Interfaces.Queries;
using AICO.Domain.Interfaces;
using AICO.Domain.Interfaces.Data;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Services;
using AICO.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AICO.Application.Commands; 
using AICO.Application.Queries;
using AICO.Infrastructure.ExternalServices;
using AICO.Application.Interfaces.Services;
using AICO.Application.Services;
using AICO.Application.Mappers;
using AICO.Domain.DTOs;
using AICO.Domain.Entities;
using AICO.Shared.Interfaces;
using AICO.Infrastructure.Data;
namespace AICO.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Domain Services Dependencies
        services.AddScoped<IAbTestService, AbTestService>();
        services.AddScoped<ICampaignService, CampaignService>();
        services.AddScoped<IVariantGenerationService, VariantGenerationService>();
        services.AddScoped<IConversionService, ConversionService>();
        services.AddScoped<IRevenueTrackingService, RevenueTrackingService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IMetricService, MetricService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IUserService, Domain.Services.UserService>();
        services.AddScoped<IWebsiteService, Domain.Services.WebsiteService>();
        services.AddScoped<ISnippetService, SnippetService>();
        services.AddScoped<IRecommendationService, RecommendationService>();

        // Domain Validation Services Dependencies
        services.AddScoped<IAbTestStateValidationService, AbTestStateValidationService>();
        services.AddScoped<ICampaignStateValidationService, CampaignStateValidationService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IStatisticalAnalysisService, StatisticalAnalysisService>();

        return services;
    }
     // Repository Dependencies
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<IVariantRepository, VariantRepository>();
        services.AddScoped<IAbTestRepository, AbTestRepository>();
        services.AddScoped<IConversionRepository, ConversionRepository>();
        services.AddScoped<IConversionEventRepository, ConversionEventRepository>();
        services.AddScoped<IRevenueRepository, RevenueRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IMetricRepository, MetricRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISnippetRepository, SnippetRepository>();
        services.AddScoped<IRecommendationRepository, RecommendationRepository>();
        services.AddScoped<IAnalysisResultRepository, AnalysisResultRepository>();
        
        // Unit of Work
        services.AddScoped<IUnitOfWork, AicoDbContext>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Command Handlers
        services.AddScoped<IAbTestCommandHandler, AbTestCommandHandler>();
        services.AddScoped<ICampaignCommandHandler, CampaignCommandHandler>();
        services.AddScoped<IVariantCommandHandler, VariantCommandHandler>();
        services.AddScoped<IConversionEventCommandHandler, ConversionEventCommandHandler>(); // Added this line
        services.AddScoped<ICommandHandler<GenerateVariantCommand, List<Variant>>, GenerateVariantCommandHandler>();

        // Query Handlers
        services.AddScoped<IAbTestQueryHandler, AbTestQueryHandler>();
        services.AddScoped<ICampaignQueryHandler, CampaignQueryHandler>();
        services.AddScoped<IVariantQueryHandler, VariantQueryHandler>();
        services.AddScoped<IConversionEventQueryHandler, ConversionEventQueryHandler>(); // Added this line

        // Application Services
        services.AddScoped<IProfitTrackingService, ProfitTrackingService>();
        services.AddScoped<IJwtService, JwtService>();
        
        // MCP Services
        services.AddScoped<IMCPAnalyticsService, MCPAnalyticsService>();
        services.AddScoped<IMCPValidationService, MCPValidationService>();
        services.AddScoped<IMCPReportingService, MCPReportingService>();
        services.AddScoped<ICurrencyConversionService, CurrencyConversionService>();
        
        // Specialized Validation Services
        services.AddScoped<IBasicInputValidationService, BasicInputValidationService>();
        services.AddScoped<IStatisticalValidationService, StatisticalValidationService>();
        services.AddScoped<ICurrencyValidationService, CurrencyValidationService>();
        services.AddScoped<IBusinessRuleValidationService, BusinessRuleValidationService>();
        
        // Statistical and Reporting Services
        services.AddScoped<IMCPStatisticalService, MCPStatisticalService>();
        services.AddScoped<IMCPReportingAnalysisService, MCPReportingAnalysisService>();

        // Register Mappers
        services.AddScoped<IMapper<Variant, VariantDto>, VariantMapper>();
        services.AddScoped<IMapper<Revenue, RevenueDto>, RevenueMapper>();
        services.AddScoped<IMapper<RevenueReportSource, RevenueReport>, RevenueReportMapper>();
        services.AddScoped<IMapper<AbTestVariant, Variant>, AbTestVariantMapper>();

        return services;
    }
     // External Services
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<Application.Interfaces.ExternalServices.IAIService, AIService>();
        services.AddScoped<Domain.Interfaces.ExternalServices.IAIService, DomainAIService>();
        services.AddScoped<Application.Interfaces.ExternalServices.IPaymentService, StripePaymentService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        
        return services;
    }
}