@echo off
setlocal EnableExtensions EnableDelayedExpansion
cd /d "%~dp0"
set "ROOT=%~dp0"
set "PATH=%ProgramFiles%\dotnet;%ProgramFiles%\nodejs;%PATH%"

echo.
echo ==============================================
echo Kier CRUD - Complete USB Setup
echo ==============================================
echo Folder: %ROOT%
echo.

where winget >nul 2>nul
if errorlevel 1 set "WINGET_AVAILABLE=0"
if not defined WINGET_AVAILABLE set "WINGET_AVAILABLE=1"

where dotnet >nul 2>nul
if errorlevel 1 (
    if "%WINGET_AVAILABLE%"=="1" (
        echo .NET SDK 8 not found. Installing it with winget...
        winget install --id Microsoft.DotNet.SDK.8 --exact --source winget --accept-source-agreements --accept-package-agreements
        if errorlevel 1 goto MissingDotnet
    ) else goto MissingDotnet
)

where node >nul 2>nul
if errorlevel 1 (
    if "%WINGET_AVAILABLE%"=="1" (
        echo Node.js not found. Installing Node.js LTS with winget...
        winget install --id OpenJS.NodeJS.LTS --exact --source winget --accept-source-agreements --accept-package-agreements
        if errorlevel 1 goto MissingNode
    ) else goto MissingNode
)

set "PATH=%ProgramFiles%\dotnet;%ProgramFiles%\nodejs;%PATH%"
where dotnet >nul 2>nul
if errorlevel 1 goto MissingDotnet
where node >nul 2>nul
if errorlevel 1 goto MissingNode
where npm.cmd >nul 2>nul
if errorlevel 1 goto MissingNode
for /f "delims=" %%N in ('where npm.cmd') do if not defined NPM_CMD set "NPM_CMD=%%N"

dotnet --list-sdks | findstr /B "8." >nul 2>nul
if errorlevel 1 goto MissingDotnet

echo.
echo .NET SDK and Node.js are ready.
echo.

echo Checking MAUI Windows workload...
dotnet workload list | findstr /I "maui-windows" >nul 2>nul
if errorlevel 1 (
    echo Installing MAUI Windows workload. This may take several minutes...
    dotnet workload install maui-windows --verbosity minimal
    if errorlevel 1 (
        echo WARNING: MAUI workload installation failed.
        echo The web systems can still be prepared, but the desktop app needs this workload.
    )
)

echo.
echo Installing root frontend packages...
call "%NPM_CMD%" install
if errorlevel 1 goto SetupFailed

echo Installing Department frontend packages...
cd /d "%ROOT%frontend"
call "%NPM_CMD%" install
if errorlevel 1 goto SetupFailed
cd /d "%ROOT%"

echo.
echo Restoring all .NET projects...
for /r "%ROOT%" %%P in (*.csproj) do (
    echo Restoring %%~fP
    dotnet restore "%%~fP"
    if errorlevel 1 goto SetupFailed
)

echo.
echo Building the Department frontend...
cd /d "%ROOT%frontend"
call "%NPM_CMD%" run build
if errorlevel 1 goto SetupFailed
cd /d "%ROOT%"

echo.
echo Checking MySQL...
if not defined DB_PASSWORD set /p "DB_PASSWORD=Enter MySQL root password (press Enter for higanbana): "
if not defined DB_PASSWORD set "DB_PASSWORD=higanbana"
set "MYSQL_CMD="
if exist "%ProgramFiles%\MySQL\MySQL Server 8.0\bin\mysqladmin.exe" set "MYSQL_CMD=%ProgramFiles%\MySQL\MySQL Server 8.0\bin\mysqladmin.exe"
if not defined MYSQL_CMD for /f "delims=" %%M in ('where mysqladmin 2^>nul') do if not defined MYSQL_CMD set "MYSQL_CMD=%%M"
if not defined MYSQL_CMD (
    echo WARNING: MySQL Server was not found.
    echo Install MySQL Server 8.x and set the root password to higanbana.
    echo The systems cannot use their databases until MySQL is running.
) else (
    "%MYSQL_CMD%" --host=localhost --port=3306 --user=root --password=%DB_PASSWORD% ping >nul 2>nul
    if errorlevel 1 (
        echo WARNING: MySQL was found but did not accept the entered root password.
        echo Check that MySQL is running and the password is correct.
    ) else (
        set "MYSQL_OK=1"
        echo MySQL is reachable with the configured credentials.
        echo Each system creates its database and tables on first start.
    )
)

if defined MYSQL_OK if exist "%ROOT%database-backups\*.sql" (
    echo.
    echo Database backups were found in database-backups.
    choice /C YN /N /M "Restore these backups now? [Y/N] "
    if not errorlevel 2 call "%ROOT%restore-all-databases.cmd"
)

echo.
echo ==============================================
echo Setup complete.
echo ==============================================
echo.
echo Start all systems with:
echo   STARTALLSYSTEMS.cmd
echo.
echo URLs after startup:
echo   Student Enrollment:       http://localhost:5173
echo   Department Financial:    http://localhost:5174
echo   SCSDS:                    http://localhost:5185
echo   Library:                  http://localhost:5190
echo   SBC Cashier:              http://localhost:5195
echo.
choice /C YN /N /M "Start all systems now? [Y/N] "
if errorlevel 2 goto Done
call "%ROOT%STARTALLSYSTEMS.cmd"

goto Done

:MissingDotnet
echo.
echo ERROR: .NET SDK 8 was not found or could not be installed.
echo Install it from https://dotnet.microsoft.com/download/dotnet/8.0
exit /b 1

:MissingNode
echo.
echo ERROR: Node.js LTS was not found or could not be installed.
echo Install it from https://nodejs.org/
exit /b 1

:SetupFailed
echo.
echo ERROR: Setup stopped because a dependency command failed.
echo Review the message above, fix the issue, and run setupall.cmd again.
exit /b 1

:Done
endlocal
exit /b 0
