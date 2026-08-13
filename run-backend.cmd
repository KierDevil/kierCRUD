@echo off
setlocal
cd /d "%~dp0backend\KierSimpleCrud.API"
set "DOTNET_CMD=dotnet"
if exist "%~dp0.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0.dotnet\dotnet.exe"
if exist "%~dp0..\Kier\.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0..\Kier\.dotnet\dotnet.exe"
"%DOTNET_CMD%" restore
"%DOTNET_CMD%" run --urls http://localhost:5000
endlocal
