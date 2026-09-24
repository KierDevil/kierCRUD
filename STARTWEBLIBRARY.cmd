@echo off
setlocal
set "PATH=%ProgramFiles%\dotnet;%PATH%"
if not defined DB_PASSWORD set /p "DB_PASSWORD=Enter MySQL root password (press Enter for higanbana): "
if not defined DB_PASSWORD set "DB_PASSWORD=higanbana"
cd /d "%~dp0library\Library.Web"
echo Starting Library System...
dotnet run --urls http://localhost:5190
