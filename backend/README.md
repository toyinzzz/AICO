# AICO Backend

<div align="center">
  <h3>🚀 AI Conversion Optimizer - Backend API Service</h3>
  
  [![.NET](https://img.shields.io/badge/.NET-8.0-512BD4.svg)](https://dotnet.microsoft.com/)
  [![C#](https://img.shields.io/badge/C%23-12.0-239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
  [![EF Core](https://img.shields.io/badge/EF_Core-8.0-0A991B.svg)](https://docs.microsoft.com/en-us/ef/core/)
</div>

## Overview

The backend API service for the AICO platform built with C# and .NET 8. This service provides the core business logic, data persistence, and API endpoints for the AICO platform, following Clean Architecture principles and SOLID design.

## Architecture

The backend follows Clean Architecture principles with the following layers:

- **API Layer**: Controllers, endpoints, middleware, and presentation logic
- **Application Layer**: Use cases, DTOs, validators, and business logic
- **Domain Layer**: Business entities, interfaces, value objects, and domain rules
- **Infrastructure Layer**: Data access, external services, and third-party integrations
- **Shared Layer**: Common utilities, helpers, and cross-cutting concerns

### Architectural Principles

- **SOLID Principles**
  - Single Responsibility: Each class has one reason to change
  - Open/Closed: Open for extension, closed for modification
  - Liskov Substitution: Derived classes must be substitutable
  - Interface Segregation: Many specific interfaces over one general
  - Dependency Inversion: Depend on abstractions, not concretions

- **Clean Architecture**
  - Independence of frameworks
  - Testability
  - Independence of UI
  - Independence of database
  - Independence of external agencies

- **Domain-Driven Design**
  - Ubiquitous language
  - Bounded contexts
  - Aggregates and entities
  - Value objects
  - Domain services

## Project Structure

```
backend/
├── src/
│   ├── AICO.API/                    # API layer
│   │   ├── Controllers/             # API controllers
│   │   ├── Middleware/              # Custom middleware
│   │   ├── Filters/                 # Action filters
│   │   └── Program.cs               # Application entry point
│   │
│   ├── AICO.Application/            # Application layer
│   │   ├── Services/                # Application services
│   │   ├── DTOs/                    # Data transfer objects
│   │   ├── Interfaces/              # Service interfaces
│   │   ├── Validators/              # FluentValidation validators
│   │   └── Mappings/                # AutoMapper profiles
│   │
│   ├── AICO.Domain/                 # Domain layer
│   │   ├── Entities/                # Domain entities
│   │   ├── ValueObjects/            # Value objects
│   │   ├── Interfaces/              # Domain interfaces
│   │   ├── Events/                  # Domain events
│   │   └── Exceptions/              # Domain exceptions
│   │
│   ├── AICO.Infrastructure/         # Infrastructure layer
│   │   ├── Data/                    # EF Core context
│   │   ├── Repositories/            # Repository implementations
│   │   ├── Services/                # External service integrations
│   │   ├── Configurations/          # EF configurations
│   │   └── Migrations/              # Database migrations
│   │
│   └── AICO.Shared/                 # Shared utilities
│       ├── Constants/               # Application constants
│       ├── Extensions/              # Extension methods
│       └── Helpers/                 # Helper classes
│
└── tests/
    ├── AICO.UnitTests/              # Unit tests
    ├── AICO.IntegrationTests/       # Integration tests
    └── AICO.ApiTests/               # API tests
```

## Technology Stack

- **Framework**: .NET 8 + ASP.NET Core
- **Language**: C# 12
- **ORM**: Entity Framework Core 8
- **IoC Container**: Microsoft.Extensions.DependencyInjection
- **Authentication**: JWT + ASP.NET Core Identity
- **API Documentation**: Swagger/OpenAPI
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Testing**: xUnit + Moq
- **Database**: PostgreSQL 15

## Key Features (Planned)

- **Website Analysis API**: Endpoints for analyzing website performance and conversion metrics
- **User Management**: Authentication, authorization, and user profile management
- **Data Persistence**: Entity Framework Core with PostgreSQL
- **AI Integration**: Communication with Python-based AI Analysis Service
- **RESTful API Design**: Well-designed API endpoints following REST principles
- **Comprehensive Testing**: Unit, integration, and API tests
- **API Documentation**: Swagger/OpenAPI documentation
- **Security**: JWT authentication, role-based authorization, and data protection

## Development (Coming Soon)

Instructions for setting up the development environment will be added as the backend is implemented, including:

- Setting up the .NET 8 development environment
- Database configuration and migrations
- Running the API locally
- Testing and debugging

## API Documentation (Planned)

The API will be documented using Swagger/OpenAPI, providing:

- Endpoint descriptions
- Request/response schemas
- Authentication requirements
- Example requests and responses

## Integration with Frontend

The backend API will provide endpoints for the React frontend to consume, handling:

- User authentication and authorization
- Website analysis and data retrieval
- AI-powered recommendations
- Data persistence and retrieval

## Deployment (Planned)

Instructions for deploying the backend API will be added as implementation progresses, covering:

- Docker containerization
- Database deployment
- Environment configuration
- Monitoring and logging 