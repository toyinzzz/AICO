@echo off
echo Setting up PostgreSQL database for AICO...

REM Navigate to the API project directory
cd /d "%~dp0..\backend\src\AICO.API"

echo Removing existing migrations...
if exist "Migrations" (
    rmdir /s /q "Migrations"
    echo Existing migrations removed.
)

echo Adding initial migration...
dotnet ef migrations add InitialCreate --verbose

if %ERRORLEVEL% NEQ 0 (
    echo Failed to add migration. Please check the error above.
    pause
    exit /b 1
)

echo Migration added successfully!
echo.
echo To update the database, run:
echo dotnet ef database update
echo.
echo Or use Docker Compose to start the full stack:
echo docker-compose -f environments/dev/docker-compose.dev.yml up --build
echo.
pause