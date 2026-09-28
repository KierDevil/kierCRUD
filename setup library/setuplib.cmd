@echo off
setlocal EnableExtensions
cd /d "%~dp0"
set "INSTALL_DIR=%LOCALAPPDATA%\KierCRUD\Library"
set "LAUNCHER=%USERPROFILE%\Desktop\Start Library.cmd"

echo ==============================================
echo KierCRUD Library - Portable Setup
echo ==============================================
echo.

where dotnet >nul 2>nul
if errorlevel 1 (
    echo .NET 8 is required but was not found.
    echo Install the .NET 8 runtime or SDK, then run this script again.
    echo Download: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

if not exist "%~dp0library-app\Library.Web.dll" (
    echo The library application payload is missing from this setup folder.
    pause
    exit /b 1
)

echo Installing to %INSTALL_DIR%...
if not exist "%INSTALL_DIR%" mkdir "%INSTALL_DIR%"
robocopy "%~dp0library-app" "%INSTALL_DIR%" /E /R:2 /W:2 /NFL /NDL /NP >nul
if errorlevel 8 (
    echo Copy failed.
    pause
    exit /b 1
)

>"%LAUNCHER%" echo @echo off
>>"%LAUNCHER%" echo setlocal
>>"%LAUNCHER%" echo set "PATH=%%ProgramFiles%%\dotnet;%%PATH%%"
>>"%LAUNCHER%" echo cd /d "%INSTALL_DIR%"
>>"%LAUNCHER%" echo if not defined DB_PASSWORD set /p "DB_PASSWORD=Enter MySQL root password (press Enter for higanbana): "
>>"%LAUNCHER%" echo if not defined DB_PASSWORD set "DB_PASSWORD=higanbana"
>>"%LAUNCHER%" echo dotnet Library.Web.dll --urls http://localhost:5190
>>"%LAUNCHER%" echo pause

echo.
echo Library installed successfully.
echo Desktop launcher created: %LAUNCHER%
echo.
echo Requirements on this PC:
echo   MySQL Server running on localhost port 3307
echo   MySQL database user root
echo   Database password matching DB_PASSWORD or higanbana
echo.
choice /C YN /N /M "Start the library now? [Y/N] "
if errorlevel 2 goto Done
call "%LAUNCHER%"

:Done
endlocal
exit /b 0