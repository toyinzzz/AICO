@echo off
echo Starting AICO Backend Tests in Docker...

REM Navigate to project root
cd /d "%~dp0.."

REM Build and run tests
docker-compose -f environments/test/docker-compose.test.yml up --build --abort-on-container-exit

REM Clean up
docker-compose -f environments/test/docker-compose.test.yml down

echo Tests completed. Check TestResults folder for detailed reports.
pause