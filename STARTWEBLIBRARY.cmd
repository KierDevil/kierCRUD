@echo off
setlocal
set "PATH=%ProgramFiles%\dotnet;%PATH%"
cd /d "%~dp0library\Library.Web"
echo Starting Library System...
dotnet run --urls http://localhost:5190
