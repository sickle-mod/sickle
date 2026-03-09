@echo off
setlocal

:: Target folder
set TARGET=%LOCALAPPDATA%\SickleMod

:: Create folder if missing
if not exist "%TARGET%" (
    mkdir "%TARGET%"
)

:: Move this batch file into the target folder if not already there
set CURRENT_DIR=%~dp0
if /I not "%CURRENT_DIR%"=="%TARGET%\" (
    echo Moving launcher to %TARGET%...
    copy "%~f0" "%TARGET%\SickleModManager.bat" >nul
    echo Relaunching from new location...
    start "" "%TARGET%\SickleModManager.bat"
    exit /b
)

echo Launcher running from %TARGET%

:: Download the PowerShell script
set SCRIPT_URL=https://raw.githubusercontent.com/sickle-mod/sickle/main/SickleModManager.ps1
set SCRIPT_FILE=%TARGET%\SickleModManager.ps1

echo Downloading SickleModManager.ps1...
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
