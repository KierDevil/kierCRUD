@echo off
setlocal
cd /d "%~dp0"

set "DOTNET_CMD="
if exist "%~dp0.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0.dotnet\dotnet.exe"
if exist "%~dp0..\Kier\.dotnet\dotnet.exe" set "DOTNET_CMD=%~dp0..\Kier\.dotnet\dotnet.exe"
if not defined DOTNET_CMD (
    where dotnet >nul 2>nul
    if not errorlevel 1 set "DOTNET_CMD=dotnet"
)

if not defined DOTNET_CMD (
    echo .NET SDK 8 x64 was not found.
    exit /b 1
)

"%DOTNET_CMD%" --list-sdks | findstr /B "8." >nul 2>nul
if errorlevel 1 (
    echo .NET SDK 8 x64 was not found.
    exit /b 1
)

set "PUBLISH_DIR=%~dp0publish\KierCRUD"
set "BACKEND_OUT=%PUBLISH_DIR%\backend"
set "APP_OUT=%PUBLISH_DIR%\app"
set "SHORTCUT_ICON=%PUBLISH_DIR%\kier_crud.ico"

if not exist "%PUBLISH_DIR%" mkdir "%PUBLISH_DIR%"
if exist "%PUBLISH_DIR%\Kier CRUD.cmd" del "%PUBLISH_DIR%\Kier CRUD.cmd"
if exist "%PUBLISH_DIR%\kiercrud.db" del "%PUBLISH_DIR%\kiercrud.db"
if exist "%PUBLISH_DIR%\kiercrud.db-shm" del "%PUBLISH_DIR%\kiercrud.db-shm"
if exist "%PUBLISH_DIR%\kiercrud.db-wal" del "%PUBLISH_DIR%\kiercrud.db-wal"
copy /Y "%~dp0assets\kier_crud.ico" "%SHORTCUT_ICON%" >nul

echo Publishing backend...
echo Close Kier CRUD first if publish says files are being used by another process.
"%DOTNET_CMD%" publish "%~dp0backend\KierSimpleCrud.API\KierSimpleCrud.API.csproj" -c Release -r win-x64 --self-contained true -o "%BACKEND_OUT%"
if errorlevel 1 exit /b 1

echo.
echo Publishing Windows app...
"%DOTNET_CMD%" publish "%~dp0mobile\KierCRUD.App\KierCRUD.App.csproj" -f net8.0-windows10.0.19041.0 -c Release -r win-x64 --self-contained true -o "%APP_OUT%"
if errorlevel 1 exit /b 1

(
echo Set shell = CreateObject^("WScript.Shell"^)
echo Set http = CreateObject^("MSXML2.XMLHTTP"^)
echo Set fso = CreateObject^("Scripting.FileSystemObject"^)
echo root = fso.GetParentFolderName^(WScript.ScriptFullName^)
echo backendOnline = False
echo On Error Resume Next
echo http.Open "GET", "http://localhost:5000/api/health", False
echo http.Send
echo If http.Status = 200 Then backendOnline = True
echo On Error GoTo 0
echo If Not backendOnline Then
echo     shell.CurrentDirectory = root ^& "\backend"
echo     shell.Run Chr^(34^) ^& root ^& "\backend\KierSimpleCrud.API.exe" ^& Chr^(34^) ^& " --urls http://localhost:5000", 0, False
echo     WScript.Sleep 3000
echo End If
echo shell.CurrentDirectory = root ^& "\app"
echo shell.Run Chr^(34^) ^& root ^& "\app\KierCRUD.App.exe" ^& Chr^(34^), 1, False
) > "%PUBLISH_DIR%\Kier CRUD.vbs"

(
echo @echo off
echo cd /d "%%~dp0"
echo start "Kier CRUD Backend" /D ".\backend" cmd /k "KierSimpleCrud.API.exe --urls http://localhost:5000"
echo timeout /t 3 /nobreak ^>nul
echo start "" ".\app\KierCRUD.App.exe"
) > "%PUBLISH_DIR%\Kier CRUD Debug.cmd"

echo.
echo Publish complete.
echo Open this folder:
echo %PUBLISH_DIR%
echo.
echo Double-click:
echo Kier CRUD.vbs
endlocal
