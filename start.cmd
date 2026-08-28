@echo off
setlocal
cd /d "%~dp0"
start "Kier CRUD Backend" cmd /k ".\run-backend.cmd"
timeout /t 3 /nobreak >nul
start "Kier CRUD App" cmd /k ".\run-app.cmd"
endlocal
