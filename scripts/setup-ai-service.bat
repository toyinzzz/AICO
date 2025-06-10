@echo off
echo Setting up AI service directory structure...

mkdir ..\ai-service 2>nul
mkdir ..\ai-service\app 2>nul
mkdir ..\ai-service\app\api 2>nul
mkdir ..\ai-service\app\api\endpoints 2>nul
mkdir ..\ai-service\app\core 2>nul
mkdir ..\ai-service\app\models 2>nul
mkdir ..\ai-service\app\services 2>nul
mkdir ..\ai-service\app\ml 2>nul
mkdir ..\ai-service\app\ml\models 2>nul
mkdir ..\ai-service\app\ml\data 2>nul
mkdir ..\ai-service\app\ml\utils 2>nul
mkdir ..\ai-service\app\db 2>nul
mkdir ..\ai-service\app\db\repositories 2>nul
mkdir ..\ai-service\app\utils 2>nul
mkdir ..\ai-service\tests 2>nul
mkdir ..\ai-service\tests\test_api 2>nul
mkdir ..\ai-service\tests\test_services 2>nul
mkdir ..\ai-service\scripts 2>nul

echo AI service directory structure created successfully!

