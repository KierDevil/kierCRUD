@echo off
setlocal
set "PATH=%ProgramFiles%\nodejs;%PATH%"
cd /d "%~dp0"

where dotnet >nul 2>nul
if errorlevel 1 (
  echo .NET SDK not found. Install .NET 8 and try again.
  exit /b 1
)

where npm >nul 2>nul
if errorlevel 1 (
  echo Node.js/npm not found. Install Node.js LTS, then rerun this script.
  echo Example: winget install OpenJS.NodeJS.LTS
  exit /b 1
)

start "" /D "%~dp0backend\DepartmentFinancialRecords.API" cmd /k "dotnet run --urls http://localhost:5001"
timeout /t 4 /nobreak >nul
start "" /D "%~dp0frontend" cmd /k "npm install && npm run dev -- --host localhost --port 5174"

echo Backend: http://localhost:5001
echo Frontend: http://localhost:5174
echo.
echo Open the browser at http://localhost:5174
endlocal
