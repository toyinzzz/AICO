@echo off
cd %~dp0
cd environments\dev
docker-compose -f docker-compose.dev.yml up --build 