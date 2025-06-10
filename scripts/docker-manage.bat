@echo off
setlocal enabledelayedexpansion

set DEV_COMPOSE=..\environments\dev\docker-compose.dev.yml
set PROD_COMPOSE=..\environments\prod\docker-compose.prod.yml
set QA_COMPOSE=..\environments\QA\docker-compose.yml

if "%1"=="" goto help

if "%1"=="dev-build" (
    echo Building development Docker images...
    docker-compose -f %DEV_COMPOSE% build
    goto end
)

if "%1"=="dev-up" (
    echo Starting development containers...
    docker-compose -f %DEV_COMPOSE% up -d
    goto end
)

if "%1"=="dev-down" (
    echo Stopping development containers...
    docker-compose -f %DEV_COMPOSE% down
    goto end
)

if "%1"=="dev-restart" (
    echo Restarting development containers...
    docker-compose -f %DEV_COMPOSE% restart
    goto end
)

if "%1"=="dev-logs" (
    echo Viewing development container logs...
    docker-compose -f %DEV_COMPOSE% logs -f
    goto end
)

if "%1"=="prod-build" (
    echo Building production Docker images...
    docker-compose -f %PROD_COMPOSE% build
    goto end
)

if "%1"=="prod-up" (
    echo Starting production containers...
    docker-compose -f %PROD_COMPOSE% up -d
    goto end
)

if "%1"=="prod-down" (
    echo Stopping production containers...
    docker-compose -f %PROD_COMPOSE% down
    goto end
)

if "%1"=="qa-build" (
    echo Building QA Docker images...
    docker-compose -f %QA_COMPOSE% build
    goto end
)

if "%1"=="qa-up" (
    echo Starting QA containers...
    docker-compose -f %QA_COMPOSE% up -d
    goto end
)

if "%1"=="qa-down" (
    echo Stopping QA containers...
    docker-compose -f %QA_COMPOSE% down
    goto end
)

if "%1"=="clean" (
    echo Stopping all containers...
    docker-compose -f %DEV_COMPOSE% down
    docker-compose -f %PROD_COMPOSE% down
    docker-compose -f %QA_COMPOSE% down
    echo Removing all AICO images...
    for /f "tokens=1" %%i in ('docker images ^| findstr aico') do (
        docker rmi %%i
    )
    echo Clean up completed.
    goto end
)

:help
echo AICO Docker Management Script
echo -----------------------------
echo Available commands:
echo   docker-manage dev-build      - Build development Docker images
echo   docker-manage dev-up         - Start development containers
echo   docker-manage dev-down       - Stop development containers
echo   docker-manage dev-restart    - Restart development containers
echo   docker-manage dev-logs       - View development container logs
echo   docker-manage prod-build     - Build production Docker images
echo   docker-manage prod-up        - Start production containers
echo   docker-manage prod-down      - Stop production containers
echo   docker-manage qa-build       - Build QA Docker images
echo   docker-manage qa-up          - Start QA containers
echo   docker-manage qa-down        - Stop QA containers
echo   docker-manage clean          - Remove all containers and images
echo   docker-manage help           - Show this help message

:end
endlocal 