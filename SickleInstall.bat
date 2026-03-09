@echo off
setlocal

echo Downloading SickleModManager.ps1...

:: Use GitHub raw URL (NOT the blob URL)
set SCRIPT_URL=https://raw.githubusercontent.com/sickle-mod/sickle/main/SickleModManager.ps1
set SCRIPT_FILE=SickleModManager.ps1

:: Download using PowerShell (works on all modern Windows)
powershell -NoLogo -NoProfile -Command ^
    "try { Invoke-WebRequest -Uri '%SCRIPT_URL%' -OutFile '%SCRIPT_FILE%' -UseBasicParsing } catch { exit 1 }"

if %errorlevel% neq 0 (
    echo Failed to download SickleModManager.ps1
    pause
    exit /b 1
)

echo Running SickleModManager.ps1...
powershell -ExecutionPolicy Bypass -File "%SCRIPT_FILE%"
exit /b 0