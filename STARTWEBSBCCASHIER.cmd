@echo off
setlocal
set "PATH=%ProgramFiles%\dotnet;%PATH%"
cd /d "%~dp0sbccashier\SBCCashier.Web"
echo Starting SBC Cashier...
dotnet run --urls http://localhost:5195
