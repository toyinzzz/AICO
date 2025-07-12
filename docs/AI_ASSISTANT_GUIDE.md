# AI Assistant Development Guide

## Quick Reference for AI Coding Assistants

### Project Context
- **Type**: Enterprise SaaS Platform for A/B Testing and MCP Analysis
- **Architecture**: Clean Architecture with .NET 8, Vue.js 3, Python FastAPI
- **Status**: MVP Development Phase (65% Production Ready)
- **Primary Goal**: Maximum Customer Profit (MCP) optimization through statistical analysis

### Critical Implementation Patterns

#### 1. Service Layer Pattern
```csharp
// Interface Definition
public interface IMCPAnalyticsService
{
    Task<MCPResult> CalculateMCPAsync(decimal revenue, decimal cost, int conversions, int visitors);
    Task<MCPAnalysisReport> GenerateAnalysisAsync(Guid abTestId, DateTime startDate, DateTime endDate);
}

// Implementation Pattern
public class MCPAnalyticsService : IMCPAnalyticsService
{
    private readonly ILogger<MCPAnalyticsService> _logger;
    private readonly IAbTestRepository _abTestRepository;
    
    public MCPAnalyticsService(ILogger<MCPAnalyticsService> logger, IAbTestRepository abTestRepository)
    {
        _logger = logger;
        _abTestRepository = abTestRepository;
    }
    
    public async Task<MCPResult> CalculateMCPAsync(decimal revenue, decimal cost, int conversions, int visitors)
    {
        try
        {
            _logger.LogInformation("Calculating MCP for revenue: {Revenue}, cost: {Cost}", revenue, cost);
            // Implementation here
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating MCP");
            throw;
        }
    }
}
```

#### 2. Repository Pattern
```csharp
// Repository Interface
public interface IAbTestRepository : IRepository<AbTest>
{
    Task<IEnumerable<AbTest>> GetActiveTestsAsync();
    Task<AbTest> GetByIdWithVariantsAsync(Guid id);
}

// Implementation with EF Core
public class AbTestRepository : Repository<AbTest>, IAbTestRepository
{
    public AbTestRepository(AICODbContext context) : base(context) { }
    
    public async Task<IEnumerable<AbTest>> GetActiveTestsAsync()
    {
        return await _context.AbTests
            .Where(t => t.Status == TestStatus.Active)
            .Include(t => t.Variants)
            .ToListAsync();
    }
}
```

#### 3. DTO Pattern
```csharp
// Request/Response DTOs
public class CreateAbTestRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CreateVariantRequest> Variants { get; set; }
}

public class MCPAnalysisResponse
{
    public Guid AbTestId { get; set; }
    public decimal OverallMCP { get; set; }
    public List<VariantComparison> VariantComparisons { get; set; }
    public MCPStatisticalInsights StatisticalInsights { get; set; }
    public DateTime GeneratedAt { get; set; }
}
```

### Common Code Generation Templates

#### API Controller Template
```csharp
[ApiController]
[Route("api/[controller]")]
public class {EntityName}Controller : ControllerBase
{
    private readonly I{EntityName}Service _{entityName}Service;
    private readonly ILogger<{EntityName}Controller> _logger;
    
    public {EntityName}Controller(I{EntityName}Service {entityName}Service, ILogger<{EntityName}Controller> logger)
    {
        _{entityName}Service = {entityName}Service;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<{EntityName}Dto>>> GetAll()
    {
        try
        {
            var result = await _{entityName}Service.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving {entityName}s");
            return StatusCode(500, "Internal server error");
        }
    }
}
```

#### Service Registration Template
```csharp
// In Program.cs or Startup.cs
services.AddScoped<I{ServiceName}Service, {ServiceName}Service>();
services.AddScoped<I{RepositoryName}Repository, {RepositoryName}Repository>();
```

### Database Patterns

#### Entity Configuration
```csharp
public class AbTestConfiguration : IEntityTypeConfiguration<AbTest>
{
    public void Configure(EntityTypeBuilder<AbTest> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.HasMany(x => x.Variants)
               .WithOne(x => x.AbTest)
               .HasForeignKey(x => x.AbTestId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
```

#### Migration Pattern
```csharp
public partial class Add{EntityName}Table : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "{TableName}",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(maxLength: 200, nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_{TableName}", x => x.Id);
            });
    }
}
```

### Testing Patterns

#### Unit Test Template
```csharp
[TestClass]
[Category("Unit")]
public class {ServiceName}ServiceTests
{
    private Mock<ILogger<{ServiceName}Service>> _mockLogger;
    private Mock<I{Repository}Repository> _mockRepository;
    private {ServiceName}Service _service;
    
    [TestInitialize]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<{ServiceName}Service>>();
        _mockRepository = new Mock<I{Repository}Repository>();
        _service = new {ServiceName}Service(_mockLogger.Object, _mockRepository.Object);
    }
    
    [TestMethod]
    public async Task {MethodName}_Should{ExpectedBehavior}_When{Condition}()
    {
        // Arrange
        var input = new {InputType} { /* properties */ };
        _mockRepository.Setup(x => x.{MethodName}(It.IsAny<{ParameterType}>()))
                      .ReturnsAsync(new {ReturnType}());
        
        // Act
        var result = await _service.{MethodName}(input);
        
        // Assert
        Assert.IsNotNull(result);
        _mockRepository.Verify(x => x.{MethodName}(It.IsAny<{ParameterType}>()), Times.Once);
    }
}
```

### Frontend Integration Patterns

#### API Service Template (TypeScript)
```typescript
// services/api/{entityName}Service.ts
import { ApiResponse, {EntityName}Dto, Create{EntityName}Request } from '@/types';
import { apiClient } from './apiClient';

export class {EntityName}Service {
  private readonly baseUrl = '/api/{entityName}';
  
  async getAll(): Promise<{EntityName}Dto[]> {
    const response = await apiClient.get<{EntityName}Dto[]>(this.baseUrl);
    return response.data;
  }
  
  async create(request: Create{EntityName}Request): Promise<{EntityName}Dto> {
    const response = await apiClient.post<{EntityName}Dto>(this.baseUrl, request);
    return response.data;
  }
}
```

### Error Handling Patterns

#### Global Exception Handler
```csharp
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            ValidationException => new { error = exception.Message, statusCode = 400 },
            NotFoundException => new { error = "Resource not found", statusCode = 404 },
            _ => new { error = "Internal server error", statusCode = 500 }
        };
        
        context.Response.StatusCode = response.statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
```

### Key Dependencies to Import

#### Backend (.NET)
```csharp
// Core Framework
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

// Domain Layer
using AICO.Domain.Entities;
using AICO.Domain.DTOs;
using AICO.Domain.ValueObjects;
using AICO.Domain.Interfaces.Repositories;

// Application Layer
using AICO.Application.Interfaces.Services;
using AICO.Application.DTOs;
using AICO.Application.Services;

// Infrastructure
using AICO.Infrastructure.Data;
using AICO.Infrastructure.Repositories;

// Testing
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
```

### Current Implementation Status

#### ✅ Completed Components
- Domain entities and value objects
- Core MCP calculation logic
- Statistical analysis services
- Repository pattern implementation
- Testing infrastructure
- Database design and EF configuration

#### ❌ Missing Critical Components
- API Controllers (Priority 1)
- Authentication middleware (Priority 1)
- OpenAI integration service (Priority 1)
- Frontend API integration (Priority 2)
- Database migrations (Priority 2)

### Build and Deployment

#### Docker Commands
```bash
# Development
make dev-build    # Build development images
make dev-up       # Start development environment
make dev-logs     # View logs

# Testing
make test-run     # Run all tests
make test-coverage # Generate coverage report

# Production
make prod-build   # Build production images
make prod-up      # Start production environment
```

#### Environment Configuration
```yaml
# Development
ASPNETCORE_ENVIRONMENT: Development
ConnectionStrings__DefaultConnection: "Host=aico-db;Database=aico_dev;Username=aico_user;Password=aico_password"
VITE_API_BASE_URL: "http://localhost:5000/api"
VITE_AI_SERVICE_URL: "http://localhost:8000"
```

This guide provides AI assistants with the essential patterns, templates, and context needed to effectively contribute to the AICO project while maintaining consistency with the established architecture and coding standards.