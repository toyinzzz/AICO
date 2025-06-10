# AICO - AI Conversion Optimizer

AICO is a comprehensive website optimization platform that analyzes websites and provides AI-driven recommendations to optimize performance, increase conversions, and enhance user experience.

## Project Structure

The project follows a microservices architecture with the following components:

```
AICO/
├── frontend/             # React + TypeScript frontend application
│   ├── src/              # Frontend source code
│   ├── public/           # Static assets
│   └── environments/     # Environment-specific configurations
│       ├── dev/          # Development environment
│       ├── prod/         # Production environment
│       └── QA/           # Quality Assurance environment
│
├── backend/              # C# + .NET 8 backend API service
│   ├── src/              # Backend source code
│   │   ├── AICO.API/     # API layer with controllers
│   │   ├── AICO.Application/  # Application layer with services
│   │   ├── AICO.Domain/  # Domain layer with entities
│   │   ├── AICO.Infrastructure/  # Infrastructure layer
│   │   └── AICO.Shared/  # Shared utilities
│   └── tests/            # Backend tests
│       ├── AICO.UnitTests/
│       ├── AICO.IntegrationTests/
│       └── AICO.ApiTests/
│
└── docs/                 # Project documentation
    ├── ARCHITECTURE.md   # Architecture documentation
    ├── PLAN.md           # Development plan
    ├── SENIOR_DEV_INSTRUCTION.md  # Developer instructions
    └── CHANGE_LOG.md     # Change log
```

## Getting Started

### Frontend Development

   ```bash
cd frontend
npm install
npm run dev
```

Or using Docker:

```bash
cd frontend
.\run-dev.bat
```

### Backend Development

```bash
cd backend
# Instructions to be added as backend is implemented
```

## Documentation

- [Architecture Documentation](docs/ARCHITECTURE.md)
- [Development Plan](docs/PLAN.md)
- [Developer Instructions](docs/SENIOR_DEV_INSTRUCTION.md)
- [Change Log](docs/CHANGE_LOG.md)

## License

This project is proprietary and confidential.