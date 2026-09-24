@echo off
setlocal
cd /d "%~dp0SCSDS.Web"
echo Starting School Canteen Student Discount System...
"C:\Program Files\dotnet\dotnet.exe" run --urls http://localhost:5185
