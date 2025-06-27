# AICO - AI Conversion Optimizer

<div align="center">
  <img src="frontend/public/placeholder.svg" alt="AICO Logo" width="200"/>
  <p><strong>Enterprise-grade platform for optimizing website conversions with AI-powered insights</strong></p>
  
  <p>
    <a href="https://reactjs.org/"><img src="https://img.shields.io/badge/React-18-blue.svg" alt="React 18"></a>
    <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8.0-512BD4.svg" alt=".NET 8"></a>
    <a href="https://fastapi.tiangolo.com/"><img src="https://img.shields.io/badge/FastAPI-0.103.1-009688.svg" alt="FastAPI"></a>
    <a href="https://www.postgresql.org/"><img src="https://img.shields.io/badge/PostgreSQL-15-336791.svg" alt="PostgreSQL 15"></a>
    <a href="https://www.docker.com/"><img src="https://img.shields.io/badge/Docker-Enabled-2496ED.svg" alt="Docker Enabled"></a>
  </p>
</div>

## Overview

AICO (AI Conversion Optimizer) is an enterprise-grade platform that leverages artificial intelligence to analyze websites and provide data-driven recommendations for optimizing conversion rates, enhancing user experience, and improving overall performance. 

Built with a modern microservices architecture, AICO follows Domain-Driven Design principles and Clean Architecture patterns to ensure scalability, maintainability, and extensibility. The platform integrates advanced analytics with machine learning algorithms to deliver actionable insights that measurably improve website conversion metrics.

AICO is designed for marketing teams, e-commerce businesses, and digital agencies seeking to maximize their online conversion rates through AI-powered optimization strategies.

### Key Features

- **Real-time Analytics**: Track user behavior, sessions, and conversion events
- **AI-Powered Insights**: Generate intelligent recommendations for conversion optimization
- **A/B Testing**: Manage and analyze conversion experiments
- **Competitor Analysis**: Monitor and compare against competitor performance
- **Multi-channel Integration**: Support for various website platforms and tools

## Architecture

AICO is built as a cloud-native microservices application with the following components:

- **Frontend Service**: React 18 + TypeScript + Vite
- **Backend API Service**: C# + .NET 8 + ASP.NET Core
- **AI Analysis Service**: Python + FastAPI
- **Database**: PostgreSQL for persistent data storage
- **Cache**: Redis for high-performance caching

For detailed architecture information, see our [Architecture Documentation](./docs/ARCHITECTURE.md).

## Documentation

Our comprehensive documentation covers all aspects of the project:

### Core Documentation
- **[Architecture](./docs/ARCHITECTURE.md)**: System design and technical architecture
- **[Business Plan](./docs/COMPREHENSIVE_BUSINESS_PLAN.md)**: Strategic direction and market positioning
- **[MVP Specification](./docs/PROFITLIFT_MVP.md)**: Detailed MVP requirements and timeline
- **[Implementation Notes](./docs/IMPLEMENTATION_NOTES.md)**: Technical implementation details

### Decision Documentation
- **[Decision Log](./docs/DECISION_LOG.md)**: Record of all architectural, technical, and business decisions
- **[Decision Documentation Guide](./docs/DECISION_DOCUMENTATION_GUIDE.md)**: Guidelines for documenting decisions
- **[Change Log](./docs/CHANGE_LOG.md)**: Historical record of project changes

### Development Documentation
- **[Next Steps](./docs/NEXT-STEP.md)**: Current development priorities and roadmap
- **[CI/CD Strategy](./docs/CI_CD_STRATEGY.md)**: Deployment and automation strategy
- **[Entity Design](./docs/ENTITY_DESIGN.md)**: Database and domain model design

> **For Contributors**: When making significant architectural, technical, or business decisions, please document them using the templates in [DECISION_LOG.md](./docs/DECISION_LOG.md) following the guidelines in [DECISION_DOCUMENTATION_GUIDE.md](./docs/DECISION_DOCUMENTATION_GUIDE.md).

## Development Setup & Status

This section outlines the current development setup progress and status for each service.

### Frontend Service (React + Vite)

- **Local Development:** The frontend can be run locally.
  - Navigate to the `frontend` directory.
  - Run `npm install` to install dependencies.
  - Run `npm run dev` to start the development server (typically on `http://localhost:8080/`).
- **Initial Setup Notes:**
  - The `frontend/run-dev.bat` script for Docker-based development was initially pointing to an incorrect path for `docker-compose.dev.yml`. This has been corrected.
  - During initial attempts, the Docker build process for the frontend (`docker-compose up --build`) was getting stuck at the "load build context" stage. As a workaround, local development via `npm run dev` was pursued and is functional.
  - Data display in the frontend is a mix: many components use mocked data, while an `mcpService.ts` exists for potential backend integration (currently configured with `mockData: true` in some usages).

### Backend API Service (.NET)

- **Database:** Configured to use PostgreSQL for development and production
- **Local Development:** The backend can be run locally or via Docker
  - Ensure PostgreSQL is running (via Docker or local installation)
  - Navigate to the `backend/src/AICO.API` directory
  - Run `dotnet restore` to restore dependencies
  - Run `dotnet ef database update` to apply migrations
  - Run `dotnet run` to start the API server (typically on `http://localhost:5000`)
- **Docker Development:** Use the provided scripts for easy setup
  - Run `scripts/docker-dev-setup.bat` to start the full stack with PostgreSQL
  - Run `scripts/setup-database.bat` to create and apply database migrations

### AI Analysis Service (Python + FastAPI)

- Setup and testing are pending.

## Project Structure

```
AICO/
├── frontend/                         # React + TypeScript frontend application
│   ├── src/                          # Frontend source code
│   │   ├── components/               # Reusable UI components
│   │   │   ├── Analysis/             # Website analysis components
│   │   │   ├── CROExpertChat/        # AI chat interface components
│   │   │   ├── Dashboard/            # Dashboard widgets and charts
│   │   │   ├── Landing/              # Landing page sections
│   │   │   ├── Layout/               # Layout components
│   │   │   ├── Pricing/              # Pricing page components
│   │   │   └── ui/                   # shadcn/ui base components
│   │   ├── hooks/                    # Custom React hooks
│   │   ├── lib/                      # Utility libraries
│   │   ├── pages/                    # Page components
│   │   ├── providers/                # Context providers
│   │   └── utils/                    # Utility functions
│   ├── public/                       # Static assets
│   ├── environments/                 # Environment-specific configurations
│   └── [config files]                # Configuration files (vite, tailwind, etc.)
│
├── backend/                          # C# + .NET 8 backend API service
│   ├── src/                          # Backend source code
│   │   ├── AICO.API/                 # API layer with controllers
│   │   │   ├── Controllers/          # API controllers
│   │   │   ├── Middleware/           # Custom middleware
│   │   │   └── Filters/              # Action filters
│   │   ├── AICO.Application/         # Application layer with services
│   │   │   ├── Services/             # Application services
│   │   │   ├── DTOs/                 # Data transfer objects
│   │   │   ├── Interfaces/           # Service interfaces
│   │   │   └── Validators/           # FluentValidation validators
│   │   ├── AICO.Domain/              # Domain layer with entities
│   │   │   ├── Entities/             # Domain entities
│   │   │   ├── ValueObjects/         # Value objects
│   │   │   ├── Events/               # Domain events
│   │   │   └── Interfaces/           # Domain interfaces
│   │   ├── AICO.Infrastructure/      # Infrastructure layer
│   │   │   ├── Data/                 # EF Core context
│   │   │   ├── Repositories/         # Repository implementations
│   │   │   └── Configurations/       # EF configurations
│   │   └── AICO.Shared/              # Shared utilities
│   └── tests/                        # Backend tests
│       ├── AICO.UnitTests/           # Unit tests
│       ├── AICO.IntegrationTests/    # Integration tests
│       └── AICO.ApiTests/            # API tests
│
├── ai-service/                       # Python + FastAPI AI analysis service
│   ├── app/                          # Service source code
│   │   ├── api/                      # FastAPI routes
│   │   ├── core/                     # Core configuration
│   │   ├── models/                   # Pydantic models
│   │   ├── services/                 # Business logic
│   │   │   ├── analysis/             # Website analysis
│   │   │   ├── scraping/             # Web scraping
│   │   │   └── ai/                   # AI/ML processing
│   │   └── utils/                    # Utility functions
│   ├── tests/                        # AI service tests
│   └── requirements.txt              # Python dependencies
│
├── docs/                             # Project documentation
│   ├── ARCHITECTURE.md               # Architecture documentation
│   ├── PLAN.md                       # Development plan
│   ├── ENTITY_DESIGN.md              # Domain model and entity relationships
│   ├── IMPLEMENTATION_NOTES.md       # Technical implementation details
│   └── CHANGE_LOG.md                 # Version history and changes
│
├── environments/                     # Environment configurations
│   ├── dev/                          # Development environment
│   ├── prod/                         # Production environment
│   └── QA/                           # Quality Assurance environment
│
└── scripts/                          # Utility scripts for development and deployment
```

Each component has its own detailed README with specific information about its structure, dependencies, and development guidelines.

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 8 SDK
- Node.js 18+ and npm/bun
- Python 3.11+
- PostgreSQL 15+ (if running locally without Docker)

### Development Setup

#### Frontend Development

```bash
# Install dependencies
cd frontend
npm install

# Start development server
npm run dev

# Or using Docker
./run-dev.bat  # Windows
./run-dev.sh   # Linux/macOS
```

For detailed frontend documentation, see the [Frontend README](./frontend/README.md).

#### Backend Development

```bash
# Option 1: Local development with PostgreSQL
cd backend/src/AICO.API
dotnet restore
dotnet ef database update
dotnet run

# Option 2: Full Docker stack (recommended)
./scripts/docker-dev-setup.bat  # Windows
./scripts/docker-dev-setup.sh   # Linux/macOS

# Option 3: Individual service via Docker
docker-compose -f environments/dev/docker-compose.dev.yml up --build
```

For detailed backend documentation, see the [Backend README](./backend/README.md).

#### AI Service Development

```bash
# Set up Python environment
cd ai-service
python -m venv venv
source venv/bin/activate  # Linux/macOS
venv\Scripts\activate     # Windows

# Install dependencies
pip install -r requirements.txt

# Run the service
uvicorn app.main:app --reload
```

## Documentation

- [Architecture Documentation](./docs/ARCHITECTURE.md) - Detailed system architecture
- [Development Plan](./docs/PLAN.md) - Project roadmap and implementation plan
- [Developer Instructions](./docs/SENIOR_DEV_INSTRUCTION.md) - Guidelines for developers
- [Entity Design](./docs/ENTITY_DESIGN.md) - Domain model and entity relationships
- [Implementation Notes](./docs/IMPLEMENTATION_NOTES.md) - Technical implementation details
- [Change Log](./docs/CHANGE_LOG.md) - Version history and changes

## Development Workflow

### Branching Strategy

- `main` - Production-ready code
- `develop` - Integration branch for feature development
- `feature/*` - Feature branches for active development
- `release/*` - Release preparation branches
- `hotfix/*` - Emergency fixes for production issues

### Continuous Integration

The project uses GitHub Actions for continuous integration, running:
- Automated builds
- Unit and integration tests
- Code quality checks
- Security scanning

### Code Quality Standards

- Comprehensive test coverage (minimum 80%)
- Static code analysis
- Code style enforcement
- Documentation requirements

## Contributing

Please follow our development guidelines outlined in the [Developer Instructions](./docs/SENIOR_DEV_INSTRUCTION.md) when contributing to this project. All contributions must adhere to our coding standards and pass all automated checks before being considered for merge.

## Security

Security vulnerabilities should be reported directly to the security team at security@example.com rather than through public issues. For more information, see our [Security Policy](./SECURITY.md).

## License

This project is proprietary and confidential. All rights reserved.

© 2023-2024 AICO Technologies

---

<div align="center">
  <p>
    <a href="https://github.com/toyinzzz/AICO/issues"><img src="https://img.shields.io/github/issues/toyinzzz/AICO.svg" alt="GitHub Issues"></a>
    <a href="https://github.com/toyinzzz/AICO/pulls"><img src="https://img.shields.io/github/issues-pr/toyinzzz/AICO.svg" alt="GitHub Pull Requests"></a>
    <a href="./LICENSE"><img src="https://img.shields.io/badge/License-Proprietary-red.svg" alt="License"></a>
  </p>
</div>