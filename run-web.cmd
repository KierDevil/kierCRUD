@echo off
REM Start the web application
cd /d "%~dp0\web\KierCRUD.Web"
"C:\Program Files\dotnet\dotnet.exe" run --urls "http://localhost:5173"
pause
