@echo off
setlocal
cd /d "%~dp0mobile\KierCRUD.App"
set "DOTNET_CMD=dotnet"
if exist "%~dp0.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0.dotnet\dotnet.exe"
if exist "%~dp0..\Kier\.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0..\Kier\.dotnet\dotnet.exe"
"%DOTNET_CMD%" run -f net8.0-windows10.0.19041.0
endlocal
