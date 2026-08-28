@echo off
setlocal
cd /d "%~dp0backend\KierSimpleCrud.API"
set "DOTNET_CMD=dotnet"
if exist "%~dp0.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0.dotnet\dotnet.exe"
if exist "%~dp0..\Kier\.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0..\Kier\.dotnet\dotnet.exe"

powershell -NoProfile -ExecutionPolicy Bypass -Command "try { $response = Invoke-WebRequest -Uri 'http://localhost:5000/api/health' -UseBasicParsing -TimeoutSec 2; if ($response.StatusCode -eq 200) { exit 0 } } catch { exit 1 }"
if not errorlevel 1 (
    echo Kier CRUD backend is already running on http://localhost:5000
    echo You can close this window.
    pause
    endlocal
    exit /b 0
)

"%DOTNET_CMD%" restore
"%DOTNET_CMD%" run --urls http://localhost:5000
endlocal
