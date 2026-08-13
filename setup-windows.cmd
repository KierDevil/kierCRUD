@echo off
setlocal
cd /d "%~dp0"

echo Kier CRUD Windows setup
echo.

set "DOTNET_CMD="
if exist "%~dp0.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0.dotnet\dotnet.exe"
if exist "%~dp0..\Kier\.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0..\Kier\.dotnet\dotnet.exe"

if not defined DOTNET_CMD (
    where dotnet >nul 2>nul
    if not errorlevel 1 set "DOTNET_CMD=dotnet"
)

if not defined DOTNET_CMD goto MissingSdk

"%DOTNET_CMD%" --list-sdks | findstr /B "8." >nul 2>nul
if errorlevel 1 goto MissingSdk

echo Installing/checking MAUI Windows workload...
echo This can take several minutes on the first setup.
echo Please wait until it says setup complete or shows an error.
echo.
"%DOTNET_CMD%" workload install maui-windows --verbosity normal
if errorlevel 1 (
    echo.
    echo MAUI Windows workload setup failed.
    echo Check your internet connection, then run setup-windows.cmd again.
    pause
    exit /b 1
)

echo.
echo Building click-to-run app folder...
echo This creates the final publish\KierCRUD folder.
echo.
call "%~dp0publish-windows.cmd"
if errorlevel 1 (
    echo.
    echo Publish failed.
    pause
    exit /b 1
)

echo.
echo Creating Desktop and Start Menu shortcuts...
call "%~dp0create-shortcuts.cmd"
if errorlevel 1 (
    echo.
    echo Shortcut creation was skipped or blocked by Windows permissions.
    echo You can still open the app from:
    echo %~dp0publish\KierCRUD\Kier CRUD.vbs
)

echo.
echo Setup complete.
echo.
echo You can now open Kier CRUD from:
echo - Desktop shortcut
echo - Windows Start Menu search
echo - publish\KierCRUD\Kier CRUD.vbs
echo.
pause
endlocal
exit /b 0

:MissingSdk
echo .NET SDK 8 x64 was not found.
echo.
echo Install .NET SDK 8 x64 first:
echo https://dotnet.microsoft.com/download/dotnet/8.0
echo.
echo After installing it, run setup-windows.cmd again.
pause
endlocal
exit /b 1
