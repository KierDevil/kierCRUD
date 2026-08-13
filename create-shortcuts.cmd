@echo off
setlocal
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0create-shortcuts.ps1"
if errorlevel 1 exit /b 1
endlocal
