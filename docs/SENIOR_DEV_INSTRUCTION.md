# AICO Project - Senior Developer Instructions for Developers

## Project Overview
AICO is a comprehensive website optimization platform built with a microservices architecture. The project emphasizes **Object-Oriented Programming (OOP)** and **SOLID principles** throughout all layers of development.

## Current Project Status

### ✅ Completed Components
- **Frontend Foundation**: React/TypeScript with modern UI components
- **Routing System**: Complete navigation structure with react-router-dom
- **UI Components**: Comprehensive component library using Shadcn UI
- **Mock Data Layer**: Temporary data structures for development
- **Project Documentation**: Architecture and planning documents

### ❌ Critical Missing Components
- **Backend API Service**: No C# backend implementation
- **AI Analysis Service**: No Python microservice
- **Database Integration**: No data persistence layer
- **Authentication System**: No user management
- **Real Analysis Engine**: Currently using mock data
- **API Service Layer**: Frontend lacks proper data fetching

## Development Roadmap - OOP & SOLID Focus

### Phase 1: C# Backend API Service (Priority 1)

#### Architecture Requirements:
1. **SOLID Principles Implementation**
   - Single Responsibility: Each class has one reason to change
   - Open/Closed: Open for extension, closed for modification
   - Liskov Substitution: Derived classes must be substitutable
   - Interface Segregation: Many specific interfaces over one general
   - Dependency Inversion: Depend on abstractions, not concretions

2. **Clean Architecture Layers**
   - **Domain Layer**: Business entities and rules
   - **Application Layer**: Use cases and business logic
   - **Infrastructure Layer**: Data access and external services
   - **API Layer**: Controllers and presentation logic

3. **Dependency Injection (IoC)**
   - Use Microsoft.Extensions.DependencyInjection
   - Register all dependencies in Program.cs
   - Constructor injection throughout

4. **Design Patterns**
   - Repository Pattern for data access
   - CQRS for command/query separation
   - Mediator Pattern for request handling

#### Implementation Tasks:

Week 1: Project Foundation
```csharp
// Example: Repository Pattern Implementation
public interface IWebsiteRepository
{
    Task<Website> GetByIdAsync(Guid id);
    Task<Website> CreateAsync(Website website);
    Task<IEnumerable<Website>> GetByUserIdAsync(Guid userId);
}

public class WebsiteRepository : IWebsiteRepository
{
    private readonly ApplicationDbContext _context;
    
    public WebsiteRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Website> GetByIdAsync(Guid id)
    {
        return await _context.Websites
            .Include(w => w.AnalysisResults)
            .FirstOrDefaultAsync(w => w.Id == id);
    }
}

// IoC Registration
services.AddScoped<IWebsiteRepository, WebsiteRepository>();
```

1. Project Structure Setup
backend/
├── src/
│   ├── AICO.API/                    # Web API layer
│   │   ├── Controllers/             # API controllers
│   │   ├── Middleware/              # Custom middleware
│   │   ├── Filters/                 # Action filters
│   │   └── Program.cs               # Application entry point
│   ├── AICO.Application/            # Application layer
│   │   ├── Services/                # Application services
│   │   ├── DTOs/                    # Data transfer objects
│   │   ├── Interfaces/              # Service interfaces
│   │   ├── Validators/              # FluentValidation validators
│   │   └── Mappings/                # AutoMapper profiles
│   ├── AICO.Domain/                 # Domain layer
│   │   ├── Entities/                # Domain entities
│   │   ├── ValueObjects/            # Value objects
│   │   ├── Interfaces/              # Domain interfaces
│   │   ├── Events/                  # Domain events
│   │   └── Exceptions/              # Domain exceptions
│   ├── AICO.Infrastructure/         # Infrastructure layer
│   │   ├── Data/                    # EF Core context
│   │   ├── Repositories/            # Repository implementations
│   │   ├── Services/                # External service integrations
│   │   ├── Configurations/          # EF configurations
│   │   └── Migrations/              # Database migrations
│   └── AICO.Shared/                 # Shared utilities
│       ├── Constants/               # Application constants
│       ├── Extensions/              # Extension methods
│       └── Helpers/                 # Helper classes
└── tests/
├── AICO.UnitTests/              # Unit tests
├── AICO.IntegrationTests/       # Integration tests
└── AICO.ApiTests/               # API tests

### AI Analysis Service Structure
ai-service/
├── app/
│   ├── api/                         # FastAPI routes
│   ├── core/                        # Core configuration
│   ├── models/                      # Pydantic models
│   ├── services/                    # Business logic
│   │   ├── analysis/                # Website analysis
│   │   ├── scraping/                # Web scraping
│   │   ├── ai/                      # AI/ML processing
│   │   └── vision/                  # Computer vision
│   └── utils/                       # Utility functions
├── tests/                           # Test files
└── requirements.txt                 # Python dependencies

## Technical Debt & Code Quality
### Current Issues:
1. Separation of Concerns : Frontend components mix UI and data logic
2. Mock Data : No real data persistence or API integration
3. Error Handling : Missing throughout the application
4. Validation : No input validation or data sanitization
5. Testing : No test coverage
### Quality Standards:
- Code Coverage : Minimum 80% for backend, 70% for frontend
- SOLID Compliance : All classes must follow SOLID principles
- API Documentation : Complete OpenAPI/Swagger specs
- Error Handling : Comprehensive error responses and logging
- Logging : Structured logging with correlation IDs
## Immediate Next Steps

1. Start with Domain Layer (C# Backend)
   
   - Create Website , User , AnalysisResult entities
   - Implement proper encapsulation and business rules
   - Follow Domain-Driven Design principles
2. Implement Repository Pattern
   
   - Create interfaces first (Interface Segregation)
   - Implement concrete repositories
   - Register in IoC container
3. Build Application Services
   
   - One service per business capability (SRP)
   - Use dependency injection
   - Implement proper error handling
4. Create API Controllers
   
   - Thin controllers (delegation to services)
   - Proper HTTP status codes
   - Input validation and sanitization
### Success Criteria:
Phase 1 Complete When:

- C# backend serves real data via REST API
- All SOLID principles demonstrated in code
- 80%+ test coverage achieved
- Frontend can authenticate and fetch real data
Phase 2 Complete When:

- Python AI service analyzes websites
- C# backend integrates with AI service
- Real recommendations replace mock data
- Performance metrics are calculated accurately
## Development Guidelines
### Code Review Checklist:
- SOLID principles followed
- Proper dependency injection used
- No tight coupling between layers
- Comprehensive error handling
- Unit tests written
- Documentation updated
### Architecture Validation:
- Clean separation of concerns
- Proper abstraction layers
- IoC container properly configured
- Database queries optimized
- API responses properly structured
## Resources & References
- Clean Architecture : Robert C. Martin
- SOLID Principles : Uncle Bob's principles
- Domain-Driven Design : Eric Evans
- C# Best Practices : Microsoft documentation
- ASP.NET Core : Official Microsoft guides
Remember : Quality over speed. Focus on building a maintainable, scalable foundation that follows OOP and SOLID principles.
Each component should be testable, loosely coupled, and highly cohesive.