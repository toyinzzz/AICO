# PostgreSQL Migration Guide

This document outlines the migration from SQL Server to PostgreSQL for the AICO project.

## Overview

The AICO project has been restructured to use PostgreSQL as the primary database for both development and production environments. This change provides better Docker support, cross-platform compatibility, and aligns with modern cloud-native practices.

## Changes Made

### 1. Backend Configuration

- **NuGet Packages**: Replaced `Microsoft.EntityFrameworkCore.SqlServer` with `Npgsql.EntityFrameworkCore.PostgreSQL`
- **Program.cs**: Updated to use `UseNpgsql()` instead of `UseSqlServer()`
- **Connection Strings**: Updated to PostgreSQL format

### 2. Database Configuration

- **Development**: PostgreSQL 15 running in Docker container
- **Connection String Format**: `Host=localhost;Database=aico_dev;Username=aico_user;Password=aico_password;Port=5432;`
- **Docker Credentials**: 
  - Username: `aico_user`
  - Password: `aico_password`
  - Database: `aico_dev`

### 3. Docker Setup

- **docker-compose.dev.yml**: Updated with PostgreSQL service configuration
- **Dockerfile.backend.dev**: Created for backend development environment
- **init-db.sql**: Database initialization script with extensions and permissions

### 4. Development Scripts

- **setup-database.bat**: Creates and applies Entity Framework migrations
- **docker-dev-setup.bat**: Complete Docker stack setup with PostgreSQL

## Migration Steps

If you're migrating from an existing SQL Server setup:

### 1. Clean Existing Migrations

```bash
cd backend/src/AICO.API
rmdir /s /q Migrations  # Windows
rm -rf Migrations       # Linux/macOS
```

### 2. Create New PostgreSQL Migrations

```bash
dotnet ef migrations add InitialCreate
```

### 3. Update Database

```bash
# For local PostgreSQL instance
dotnet ef database update

# Or use Docker stack
docker-compose -f environments/dev/docker-compose.dev.yml up --build
```

## Development Workflow

### Option 1: Full Docker Stack (Recommended)

```bash
# Start all services including PostgreSQL
./scripts/docker-dev-setup.bat
```

This will start:
- Frontend on http://localhost:8080
- Backend API on http://localhost:5000
- PostgreSQL on localhost:5432

### Option 2: Local Development

1. Start PostgreSQL (Docker or local installation)
2. Update connection string in `appsettings.Development.json`
3. Run migrations: `dotnet ef database update`
4. Start backend: `dotnet run`

## Connection Strings

### Development (Local)
```
Host=localhost;Database=aico_dev;Username=aico_user;Password=aico_password;Port=5432;
```

### Development (Docker)
```
Host=aico-db;Database=aico_dev;Username=aico_user;Password=aico_password;Port=5432;
```

## Database Features

The PostgreSQL setup includes:

- **UUID Extension**: For generating unique identifiers
- **pgcrypto Extension**: For cryptographic functions
- **Proper Permissions**: User has full access to the development database
- **Data Persistence**: Docker volume for data persistence across container restarts

## Troubleshooting

### Connection Issues

1. **Check PostgreSQL is running**:
   ```bash
   docker-compose -f environments/dev/docker-compose.dev.yml ps
   ```

2. **Check connection string format**:
   - Ensure using PostgreSQL format, not SQL Server format
   - Verify credentials match Docker configuration

3. **Check database exists**:
   ```bash
   docker exec -it aico-db psql -U aico_user -d aico_dev
   ```

### Migration Issues

1. **Remove old migrations**: Delete the `Migrations` folder
2. **Create fresh migration**: `dotnet ef migrations add InitialCreate`
3. **Apply migration**: `dotnet ef database update`

### Docker Issues

1. **Clean containers and volumes**:
   ```bash
   docker-compose -f environments/dev/docker-compose.dev.yml down -v
   docker system prune -f
   ```

2. **Rebuild from scratch**:
   ```bash
   ./scripts/docker-dev-setup.bat
   ```

## Benefits of PostgreSQL

1. **Better Docker Support**: Official PostgreSQL images are well-maintained
2. **Cross-Platform**: Works consistently across Windows, macOS, and Linux
3. **Advanced Features**: Better JSON support, full-text search, and extensions
4. **Performance**: Generally better performance for complex queries
5. **Open Source**: No licensing costs or restrictions
6. **Cloud Ready**: Excellent support in cloud platforms (AWS RDS, Azure Database, etc.)

## Next Steps

1. Test the full Docker stack setup
2. Verify all Entity Framework operations work correctly
3. Update any integration tests to use PostgreSQL
4. Consider adding database seeding scripts for development data
5. Plan production deployment with managed PostgreSQL service

## Support

If you encounter issues with the PostgreSQL migration:

1. Check this documentation first
2. Review the Docker logs: `docker-compose logs`
3. Verify your local environment meets the prerequisites
4. Consult the main [README.md](../README.md) for general setup instructions