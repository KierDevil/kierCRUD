@echo off
setlocal
set "PATH=%ProgramFiles%\dotnet;%PATH%"
cd /d "%~dp0scsds\SCSDS.Web"
echo Starting School Canteen Student Discount System (SCSDS)...
dotnet run --urls http://localhost:5185
