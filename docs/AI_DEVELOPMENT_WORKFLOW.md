# AI Assistant Development Workflow

## 🎯 Purpose

This guide provides AI coding assistants with a structured workflow for contributing to the AICO project effectively. Follow these patterns to ensure consistent, high-quality code generation and modifications.

## 🚀 Getting Started Checklist

### Before Any Code Changes

1. **Read Project Context**
   - [ ] Review [`.claude.md`](../.claude.md) for project overview
   - [ ] Check [`AI_ASSISTANT_GUIDE.md`](./AI_ASSISTANT_GUIDE.md) for current status
   - [ ] Understand the MCP (Maximum Customer Profit) business logic

2. **Understand the Request**
   - [ ] Identify the feature/component being requested
   - [ ] Determine which layer(s) will be affected (Domain, Application, Infrastructure, API, Frontend)
   - [ ] Check for existing similar implementations

3. **Review Architecture**
   - [ ] Confirm Clean Architecture patterns
   - [ ] Identify required dependencies and interfaces
   - [ ] Plan the implementation approach

## 🏗️ Development Workflow

### 1. Backend Development (C# .NET 8)

#### Step 1: Domain Layer (if needed)
```
📁 AICO.Domain/
├── Entities/          # Add new entity
├── ValueObjects/      # Add value objects
├── Enums/            # Add enumerations
└── Events/           # Add domain events
```

**Process:**
1. Create entity with proper navigation properties
2. Add to `AicoDbContext` if new entity
3. Create migration if database changes needed

#### Step 2: Application Layer
```
📁 AICO.Application/
├── Features/
│   └── {Feature}/
│       ├── Commands/   # CQRS commands
│       ├── Queries/    # CQRS queries
│       └── Validators/ # FluentValidation
├── Interfaces/        # Service interfaces
├── Services/          # Business logic services
└── DTOs/             # Data transfer objects
```

**Process:**
1. Create service interface in `Interfaces/`
2. Implement service in `Services/`
3. Create CQRS commands/queries in `Features/`
4. Add FluentValidation validators
5. Register services in `Program.cs`

#### Step 3: Infrastructure Layer
```
📁 AICO.Infrastructure/
├── Data/
│   ├── Configurations/ # EF configurations
│   └── Repositories/   # Repository implementations
├── Services/          # External service implementations
└── Migrations/        # EF migrations
```

**Process:**
1. Create repository interface and implementation
2. Add EF configuration for new entities
3. Create and run migrations
4. Register repositories in `Program.cs`

#### Step 4: API Layer
```
📁 AICO.API/
├── Controllers/       # API controllers
├── Middleware/        # Custom middleware
└── DTOs/             # API-specific DTOs
```

**Process:**
1. Create controller with proper routing
2. Add authentication/authorization attributes
3. Implement proper error handling
4. Add API documentation attributes

### 2. Frontend Development (Vue.js 3)

#### Step 1: Types and Interfaces
```
📁 frontend/src/
├── types/            # TypeScript interfaces
└── services/         # API service classes
```

#### Step 2: Store Management (Pinia)
```
📁 frontend/src/stores/
└── {feature}Store.ts # Pinia store
```

#### Step 3: Components
```
📁 frontend/src/
├── components/       # Reusable components
├── views/           # Page components
└── router/          # Vue Router configuration
```

## 📋 Code Generation Templates

### Backend Service Creation

1. **Interface Definition**
   ```csharp
   // Use template from CODE_TEMPLATES.md
   public interface I{Feature}Service
   {
       Task<{Feature}Result> Process{Feature}Async({Feature}Request request);
   }
   ```

2. **Service Implementation**
   ```csharp
   // Follow logging, error handling, and dependency injection patterns
   public class {Feature}Service : I{Feature}Service
   {
       // Implementation with proper error handling and logging
   }
   ```

3. **Repository Pattern**
   ```csharp
   // Extend generic repository with specific methods
   public interface I{Feature}Repository : IRepository<{Feature}>
   {
       Task<IEnumerable<{Feature}>> GetBy{Property}Async(int {property}Id);
   }
   ```

4. **API Controller**
   ```csharp
   // Use MediatR pattern with proper error handling
   [ApiController]
   [Route("api/[controller]")]
   [Authorize]
   public class {Feature}Controller : ControllerBase
   {
       // Implementation with proper HTTP methods and responses
   }
   ```

### Frontend Component Creation

1. **Vue Component**
   ```vue
   <!-- Use composition API with TypeScript -->
   <template>
     <!-- Template with proper accessibility -->
   </template>
   
   <script setup lang="ts">
   // Composition API with proper type safety
   </script>
   
   <style scoped>
   <!-- Scoped styles following design system -->
   </style>
   ```

2. **Pinia Store**
   ```typescript
   // Use composition API pattern with proper error handling
   export const use{Feature}Store = defineStore('{feature}', () => {
     // State, getters, and actions
   })
   ```

## 🧪 Testing Strategy

### Backend Testing

1. **Unit Tests**
   - Test business logic in services
   - Mock dependencies using Moq
   - Follow AAA pattern (Arrange, Act, Assert)

2. **Integration Tests**
   - Test API endpoints
   - Use TestServer and in-memory database
   - Test complete request/response cycle

3. **Repository Tests**
   - Test data access logic
   - Use in-memory database
   - Test complex queries

### Frontend Testing

1. **Component Tests**
   - Test component behavior
   - Mock API calls
   - Test user interactions

2. **Store Tests**
   - Test state management
   - Test API integration
   - Test error handling

## 🔍 Code Review Checklist

### Backend Code

- [ ] **Architecture**: Follows Clean Architecture principles
- [ ] **SOLID Principles**: Single responsibility, dependency inversion
- [ ] **Error Handling**: Proper exception handling and logging
- [ ] **Validation**: Input validation using FluentValidation
- [ ] **Security**: Proper authorization and input sanitization
- [ ] **Performance**: Efficient database queries and caching
- [ ] **Testing**: Adequate unit and integration test coverage
- [ ] **Documentation**: XML comments for public APIs

### Frontend Code

- [ ] **TypeScript**: Proper type definitions and type safety
- [ ] **Composition API**: Using Vue 3 composition API patterns
- [ ] **State Management**: Proper Pinia store usage
- [ ] **Error Handling**: User-friendly error messages
- [ ] **Accessibility**: ARIA labels and keyboard navigation
- [ ] **Performance**: Lazy loading and code splitting
- [ ] **Styling**: Consistent design system usage

## 🚨 Common Pitfalls to Avoid

### Backend

1. **❌ Don't**: Create services without interfaces
   **✅ Do**: Always create interface first, then implementation

2. **❌ Don't**: Put business logic in controllers
   **✅ Do**: Keep controllers thin, business logic in services

3. **❌ Don't**: Use direct database queries in controllers
   **✅ Do**: Use repository pattern and CQRS

4. **❌ Don't**: Ignore error handling
   **✅ Do**: Implement comprehensive error handling and logging

5. **❌ Don't**: Skip validation
   **✅ Do**: Validate all inputs using FluentValidation

### Frontend

1. **❌ Don't**: Use Options API
   **✅ Do**: Use Composition API for all new components

2. **❌ Don't**: Mutate props directly
   **✅ Do**: Emit events for parent communication

3. **❌ Don't**: Put API calls in components
   **✅ Do**: Use Pinia stores for state management and API calls

4. **❌ Don't**: Ignore TypeScript errors
   **✅ Do**: Fix all TypeScript errors and maintain type safety

5. **❌ Don't**: Skip error handling in stores
   **✅ Do**: Handle errors gracefully with user feedback

## 📊 MCP-Specific Patterns

### MCP Calculation Service
```csharp
public async Task<decimal> CalculateMCPAsync(MCPRequest request)
{
    // 1. Validate input parameters
    // 2. Fetch conversion data from repository
    // 3. Calculate MCP using business formula
    // 4. Log calculation details
    // 5. Return result with metadata
}
```

### Statistical Analysis Integration
```csharp
public async Task<StatisticalResult> AnalyzeSignificanceAsync(
    int controlVariantId, 
    int testVariantId, 
    DateTime startDate, 
    DateTime endDate)
{
    // 1. Gather data for both variants
    // 2. Calculate statistical metrics
    // 3. Determine significance
    // 4. Return comprehensive analysis
}
```

### A/B Test Management
```csharp
public async Task<ABTestResult> CreateABTestAsync(CreateABTestRequest request)
{
    // 1. Validate test configuration
    // 2. Create test and variants
    // 3. Set up traffic allocation
    // 4. Initialize tracking
    // 5. Return test details
}
```

## 🔧 Development Environment Setup

### Prerequisites Check
```bash
# Verify required tools
dotnet --version  # Should be 8.0+
node --version    # Should be 18+
docker --version  # For containerization
psql --version    # PostgreSQL client
```

### Quick Start Commands
```bash
# Clone and setup
git clone <repository>
cd AICO

# Backend setup
cd backend
dotnet restore
dotnet build

# Frontend setup
cd ../frontend
npm install
npm run dev

# Database setup
docker-compose up -d postgres
dotnet ef database update
```

## 📈 Performance Guidelines

### Database Optimization
- Use appropriate indexes for query patterns
- Implement pagination for large datasets
- Use projection for read-only queries
- Consider caching for frequently accessed data

### API Optimization
- Implement response caching where appropriate
- Use async/await consistently
- Implement proper pagination
- Add compression for large responses

### Frontend Optimization
- Implement lazy loading for routes
- Use virtual scrolling for large lists
- Optimize bundle size with tree shaking
- Implement proper caching strategies

## 🎯 Success Metrics

When implementing features, ensure:

1. **Code Quality**: Follows established patterns and conventions
2. **Test Coverage**: Adequate unit and integration tests
3. **Performance**: Meets performance requirements
4. **Security**: Follows security best practices
5. **Documentation**: Properly documented for future maintenance
6. **User Experience**: Intuitive and accessible interface

## 📞 Getting Help

When stuck or uncertain:

1. **Check Documentation**: Review relevant documentation files
2. **Examine Existing Code**: Look for similar implementations
3. **Follow Patterns**: Use established patterns and templates
4. **Ask Specific Questions**: Provide context and specific requirements

This workflow ensures consistent, high-quality contributions to the AICO project while maintaining architectural integrity and development best practices.