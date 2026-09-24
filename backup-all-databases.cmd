@echo off
setlocal EnableExtensions
cd /d "%~dp0"
set "MYSQLDUMP="
set "PATH=%ProgramFiles%\MySQL\MySQL Server 8.0\bin;%PATH%"
for /f "delims=" %%M in ('where mysqldump 2^>nul') do if not defined MYSQLDUMP set "MYSQLDUMP=%%M"
if not defined MYSQLDUMP (
    echo mysqldump was not found. Install MySQL Server 8.x first.
    exit /b 1
)
if not defined DB_PASSWORD set /p "DB_PASSWORD=Enter MySQL root password (press Enter for higanbana): "
if not defined DB_PASSWORD set "DB_PASSWORD=higanbana"

if not exist "%~dp0database-backups" mkdir "%~dp0database-backups"
for %%D in (kiercrud department_financial_records scsds library sbccashier) do (
    echo Backing up %%D...
    "%MYSQLDUMP%" --host=localhost --port=3307 --user=root --password=%DB_PASSWORD% --single-transaction --routines --events --triggers --databases %%D > "%~dp0database-backups\%%D.sql"
    if errorlevel 1 (
        echo Backup failed for %%D.
        exit /b 1
    )
)

echo.
echo Database backups created in:
echo %~dp0database-backups
endlocal
exit /b 0
