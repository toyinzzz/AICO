@echo off
echo Starting AICO Backend Tests with Coverage in Docker...

REM Navigate to project root
cd /d "%~dp0.."

REM Create TestResults directory if it doesn't exist
if not exist "backend\TestResults" mkdir "backend\TestResults"

REM Run tests with coverage
docker-compose -f environments/test/docker-compose.test.yml up --build aico-backend-tests

REM Generate coverage report
docker run --rm -v "%cd%\backend\TestResults:/app/TestResults" mcr.microsoft.com/dotnet/sdk:8.0 sh -c "dotnet tool install --global dotnet-reportgenerator-globaltool && /root/.dotnet/tools/reportgenerator -reports:/app/TestResults/**/coverage.cobertura.xml -targetdir:/app/TestResults/CoverageReport -reporttypes:Html"

REM Clean up
docker-compose -f environments/test/docker-compose.test.yml down

echo Tests completed with coverage report generated.
echo Open backend/TestResults/CoverageReport/index.html to view coverage.
pause