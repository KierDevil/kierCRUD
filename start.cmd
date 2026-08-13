@echo off
setlocal
cd /d "%~dp0"
start "Kier CRUD Backend" cmd /k ".\run-backend.cmd"
start "Kier CRUD App" cmd /k ".\run-app.cmd"
endlocal
