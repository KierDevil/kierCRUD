@echo off
setlocal EnableExtensions
cd /d "%~dp0"
set "MYSQL="
set "PATH=%ProgramFiles%\MySQL\MySQL Server 8.0\bin;%PATH%"
for /f "delims=" %%M in ('where mysql 2^>nul') do if not defined MYSQL set "MYSQL=%%M"
if not defined MYSQL (
    echo mysql.exe was not found. Install MySQL Server 8.x first.
    exit /b 1
)

if not exist "%~dp0database-backups" (
    echo No database-backups folder was found.
    echo Run backup-all-databases.cmd on the original PC first.
    exit /b 1
)

for %%D in (kiercrud department_financial_records scsds library sbccashier) do (
    if exist "%~dp0database-backups\%%D.sql" (
        echo Restoring %%D...
        "%MYSQL%" --host=localhost --port=3306 --user=root --password=higanbana < "%~dp0database-backups\%%D.sql"
        if errorlevel 1 (
            echo Restore failed for %%D.
            exit /b 1
        )
    ) else (
        echo No backup found for %%D. Skipping.
    )
)

echo.
echo Database restore complete.
endlocal
exit /b 0
