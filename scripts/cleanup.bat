@echo off
echo Cleaning up duplicate files from the root directory...

REM Remove frontend configuration files
del vite.config.ts
del tsconfig.json
del tsconfig.app.json
del tsconfig.node.json
del tailwind.config.ts
del postcss.config.js
del package.json
del package-lock.json
del eslint.config.js
del components.json
del bun.lockb
del index.html
del run-dev.bat

REM Remove unnecessary files
del bash.exe.stackdump

echo Removing source and public directories (already moved to frontend)...
rmdir /s /q src
rmdir /s /q public

echo Root directory cleanup completed successfully! 