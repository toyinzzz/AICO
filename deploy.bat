@echo off
echo ===== AICO Deployment Script =====
echo.

:: Check if build exists
if not exist "dist" (
  echo Error: Build directory not found!
  echo Please run build-prod.bat first to create a production build.
  exit /b 1
)

:: Ask for deployment target
echo Where would you like to deploy?
echo 1. Local web server (copy to specified directory)
echo 2. FTP server (requires FTP credentials)
echo 3. Create deployment package only
echo.

set /p deploy_target=Enter option (1-3): 

if "%deploy_target%"=="1" (
  :: Local web server deployment
  echo.
  echo === Local Web Server Deployment ===
  set /p deploy_dir=Enter the destination directory: 
  
  if not exist "%deploy_dir%" (
    echo Creating directory %deploy_dir%...
    mkdir "%deploy_dir%"
  ) else (
    echo Cleaning existing files in %deploy_dir%...
    del /q "%deploy_dir%\*"
  )
  
  echo Copying files to %deploy_dir%...
  xcopy "dist\*" "%deploy_dir%" /s /e /y
  
  echo.
  echo Deployment completed successfully!
  echo Files deployed to: %deploy_dir%
)

if "%deploy_target%"=="2" (
  :: FTP deployment
  echo.
  echo === FTP Server Deployment ===
  set /p ftp_server=Enter FTP server address: 
  set /p ftp_user=Enter FTP username: 
  set /p ftp_pass=Enter FTP password: 
  set /p ftp_dir=Enter remote directory (leave blank for root): 
  
  echo Creating FTP script...
  echo open %ftp_server% > ftpcmd.dat
  echo %ftp_user% >> ftpcmd.dat
  echo %ftp_pass% >> ftpcmd.dat
  echo binary >> ftpcmd.dat
  if not "%ftp_dir%"=="" echo cd %ftp_dir% >> ftpcmd.dat
  echo mput dist\* >> ftpcmd.dat
  echo quit >> ftpcmd.dat
  
  echo Uploading files to FTP server...
  ftp -s:ftpcmd.dat
  
  echo Cleaning up...
  del ftpcmd.dat
  
  echo.
  echo Deployment completed successfully!
  echo Files uploaded to: %ftp_server%/%ftp_dir%
)

if "%deploy_target%"=="3" (
  :: Create deployment package
  echo.
  echo === Creating Deployment Package ===
  
  if not exist "deploy" mkdir deploy
  if exist "deploy\aico-frontend.zip" del "deploy\aico-frontend.zip"
  
  echo Creating zip package...
  powershell -command "Compress-Archive -Path dist\* -DestinationPath deploy\aico-frontend.zip"
  
  echo.
  echo Deployment package created at 'deploy\aico-frontend.zip'
)

echo.
echo ===== Deployment process completed! ===== 