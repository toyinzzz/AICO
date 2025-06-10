@echo off
echo ===== AICO Docker Production Deployment =====
echo.

echo Building and deploying Docker production environment...
echo.

cd environments\prod

echo Building Docker production image...
docker-compose -f docker-compose.prod.yml build

if %ERRORLEVEL% NEQ 0 (
  echo Error: Docker build failed!
  exit /b %ERRORLEVEL%
)

echo.
echo Starting Docker production containers...
docker-compose -f docker-compose.prod.yml up -d

if %ERRORLEVEL% NEQ 0 (
  echo Error: Docker deployment failed!
  exit /b %ERRORLEVEL%
)

echo.
echo ===== Docker Production Deployment Information =====
echo Deployment Date: %DATE%
echo Deployment Time: %TIME%
echo.
echo Frontend URL: http://localhost
echo.
echo To stop the production environment:
echo   cd environments\prod
echo   docker-compose -f docker-compose.prod.yml down
echo ================================================
echo.

cd ..\..

echo Production deployment completed successfully! 