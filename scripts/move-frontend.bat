@echo off
echo Creating frontend directory structure...
mkdir frontend

echo Moving frontend files to frontend directory...
robocopy src frontend\src /E
robocopy public frontend\public /E
copy components.json frontend\
copy eslint.config.js frontend\
copy index.html frontend\
copy package.json frontend\
copy package-lock.json frontend\
copy postcss.config.js frontend\
copy tailwind.config.ts frontend\
copy tsconfig.app.json frontend\
copy tsconfig.json frontend\
copy tsconfig.node.json frontend\
copy vite.config.ts frontend\
copy bun.lockb frontend\

echo Moving Docker development files...
mkdir frontend\environments
robocopy environments\dev frontend\environments\dev /E
robocopy environments\prod frontend\environments\prod /E
robocopy environments\QA frontend\environments\QA /E

echo Moving build and deployment scripts...
copy build-prod.bat frontend\
copy deploy.bat frontend\
copy docker-prod-deploy.bat frontend\
copy run-dev.bat frontend\

echo Frontend files moved successfully!
