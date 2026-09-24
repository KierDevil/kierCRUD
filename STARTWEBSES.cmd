@echo off
set "PATH=%ProgramFiles%\dotnet;%PATH%"
if not defined DB_PASSWORD set /p "DB_PASSWORD=Enter MySQL root password (press Enter for higanbana): "
if not defined DB_PASSWORD set "DB_PASSWORD=higanbana"
REM Start both backend and web application
title Kier CRUD - Multi-window Launcher

REM Start backend in one window
echo Starting Backend API...
start "Kier CRUD - Backend API" cmd /k "cd /d "%~dp0\backend\KierSimpleCrud.API" && dotnet run --urls http://localhost:5000"

REM Wait a moment for backend to start
timeout /t 3 /nobreak

REM Start web app in another window
echo Starting Web Application...
start "Kier CRUD - Web App" cmd /k "cd /d "%~dp0\web\KierCRUD.Web" && dotnet run --urls http://localhost:5173"

REM Keep this window open
timeout /t 5
echo.
echo Backend and Web App are starting...
echo Backend API: http://localhost:5000
echo Web App: http://localhost:5173
echo.
pause
