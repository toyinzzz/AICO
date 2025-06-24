using AICO.Application.Interfaces.Commands;
using AICO.Application.Interfaces.ExternalServices;
using AICO.Application.Interfaces.Queries;
using AICO.Domain.Interfaces;
using AICO.Domain.Interfaces.Repositories;
using AICO.Domain.Interfaces.Services;
using AICO.Domain.Services;
using AICO.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AICO.Application.Commands; // Corrected this line
using AICO.Application.Queries;
using AICO.Infrastructure.ExternalServices;
using AICO.Application.Interfaces.Services;
using AICO.Application.Services; // Corrected this line
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
        // Domain Services
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
        // Domain Validation Services
        services.AddScoped<IAbTestStateValidationService, AbTestStateValidationService>();
        services.AddScoped<ICampaignStateValidationService, CampaignStateValidationService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<IVariantRepository, VariantRepository>();
        services.AddScoped<IAbTestRepository, AbTestRepository>();
        services.AddScoped<IConversionRepository, ConversionRepository>();
        services.AddScoped<IRevenueRepository, RevenueRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IMetricRepository, MetricRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();
        services.AddScoped<ISnippetRepository, SnippetRepository>();
        services.AddScoped<IRecommendationRepository, RecommendationRepository>();
        services.AddScoped<IAnalysisResultRepository, AnalysisResultRepository>();
        services.AddScoped<IVariantRepository, VariantRepository>();

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

        // Register Mappers
        services.AddScoped<IMapper<Variant, VariantDto>, VariantMapper>();
            services.AddScoped<IMapper<Revenue, RevenueDto>, RevenueMapper>();
            services.AddScoped<IMapper<RevenueReportSource, RevenueReport>, RevenueReportMapper>();
            services.AddScoped<IMapper<AbTestVariant, Variant>, AbTestVariantMapper>();
            services.AddScoped<IUnitOfWork, AicoDbContext>();

        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<Domain.Interfaces.ExternalServices.IAIService, AIService>();
        services.AddScoped<Application.Interfaces.ExternalServices.IPaymentService, StripePaymentService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        
        return services;
    }
}