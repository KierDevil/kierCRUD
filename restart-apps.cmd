@echo off
echo Stopping applications...
taskkill /IM KierCRUD.Web.exe /F 2>nul
taskkill /IM KierSimpleCrud.API.exe /F 2>nul
timeout /t 2

echo Starting apps with new code...
cd /d c:\kierCRUD
start "Kier CRUD - Backend API" cmd /k "cd /d c:\kierCRUD\backend\KierSimpleCrud.API && ""C:\Program Files\dotnet\dotnet.exe"" run"
timeout /t 3
start "Kier CRUD - Web App" cmd /k "cd /d c:\kierCRUD\web\KierCRUD.Web && ""C:\Program Files\dotnet\dotnet.exe"" run --urls http://localhost:5173"
timeout /t 5
echo Done! Refresh http://localhost:5173 in browser
pause
