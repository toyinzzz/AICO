@echo off
echo ===== AICO Production Build Script =====
echo.

echo Building frontend application...
call npm run build

if %ERRORLEVEL% NEQ 0 (
  echo Error: Build failed!
  exit /b %ERRORLEVEL%
)

echo.
echo Build successful! Production files are in the 'dist' directory.
echo.

echo ===== Build Information =====
echo Build Date: %DATE%
echo Build Time: %TIME%
echo ===========================
echo.

echo Do you want to create a deployment package? (Y/N)
set /p deploy_choice=

if /i "%deploy_choice%"=="Y" (
  echo.
  echo Creating deployment package...
  
  if not exist "deploy" mkdir deploy
  if exist "deploy\aico-frontend.zip" del "deploy\aico-frontend.zip"
  
  powershell -command "Compress-Archive -Path dist\* -DestinationPath deploy\aico-frontend.zip"
  
  echo.
  echo Deployment package created at 'deploy\aico-frontend.zip'
)

echo.
echo Build process completed! 