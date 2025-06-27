@echo off
echo AICO Development Environment Setup with PostgreSQL
echo ================================================

REM Navigate to project root
cd /d "%~dp0.."

echo Stopping any existing containers...
docker-compose -f environments/dev/docker-compose.dev.yml down

echo Removing existing volumes (this will delete database data)...
set /p confirm="Do you want to remove existing database data? (y/N): "
if /i "%confirm%"=="y" (
    docker volume rm aico_postgres_dev_data 2>nul
    echo Database volume removed.
) else (
    echo Keeping existing database data.
)

echo Building and starting services...
docker-compose -f environments/dev/docker-compose.dev.yml up --build -d

echo Waiting for services to start...
timeout /t 10 /nobreak >nul

echo Checking service status...
docker-compose -f environments/dev/docker-compose.dev.yml ps

echo.
echo Services should be available at:
echo - Frontend: http://localhost:8080
echo - Backend API: http://localhost:5000
echo - PostgreSQL: localhost:5432
echo.
echo To view logs: docker-compose -f environments/dev/docker-compose.dev.yml logs -f
echo To stop services: docker-compose -f environments/dev/docker-compose.dev.yml down
echo.
pause