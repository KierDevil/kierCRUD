@echo off
setlocal
set "PATH=%ProgramFiles%\dotnet;%ProgramFiles%\nodejs;%PATH%"
cd /d "%~dp0"

echo Starting Student Enrollment System...
start "SES - Student Enrollment System" cmd /k call "%~dp0STARTWEBSES.cmd"
timeout /t 2 /nobreak >nul

echo Starting School Canteen Student Discount System...
start "SCSDS" cmd /k call "%~dp0STARTWEBSCSDS.cmd"

echo Starting Library System...
start "Library System" cmd /k call "%~dp0STARTWEBLIBRARY.cmd"
echo Starting SBC Cashier...
start "SBC Cashier" cmd /k call "%~dp0STARTWEBSBCCASHIER.cmd"

echo Starting Department Financial Records...
start "Department Financial Records" cmd /k call "%~dp0start.cmd"

echo.
echo All systems are starting in separate windows.
echo SES:         http://localhost:5173
echo SCSDS:       http://localhost:5185
echo Library:     http://localhost:5190
echo SBC Cashier: http://localhost:5195
echo Department:  http://localhost:5174
echo.
pause
