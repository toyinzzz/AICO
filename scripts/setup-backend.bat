@echo off
echo Setting up backend directory structure...

mkdir backend\src\AICO.API
mkdir backend\src\AICO.Application
mkdir backend\src\AICO.Domain
mkdir backend\src\AICO.Infrastructure
mkdir backend\src\AICO.Shared
mkdir backend\tests
mkdir backend\tests\AICO.UnitTests
mkdir backend\tests\AICO.IntegrationTests
mkdir backend\tests\AICO.ApiTests

echo Creating basic domain entities...
mkdir backend\src\AICO.Domain\Entities
mkdir backend\src\AICO.Domain\Interfaces
mkdir backend\src\AICO.Domain\ValueObjects
mkdir backend\src\AICO.Domain\Events
mkdir backend\src\AICO.Domain\Exceptions

echo Creating application layer structure...
mkdir backend\src\AICO.Application\Services
mkdir backend\src\AICO.Application\DTOs
mkdir backend\src\AICO.Application\Interfaces
mkdir backend\src\AICO.Application\Validators
mkdir backend\src\AICO.Application\Mappings

echo Creating infrastructure layer structure...
mkdir backend\src\AICO.Infrastructure\Data
mkdir backend\src\AICO.Infrastructure\Repositories
mkdir backend\src\AICO.Infrastructure\Services
mkdir backend\src\AICO.Infrastructure\Configurations
mkdir backend\src\AICO.Infrastructure\Migrations

echo Creating API layer structure...
mkdir backend\src\AICO.API\Controllers
mkdir backend\src\AICO.API\Middleware
mkdir backend\src\AICO.API\Filters

echo Backend directory structure created successfully! 