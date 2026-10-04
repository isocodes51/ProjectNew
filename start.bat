@echo off
REM ===================================================
REM  Turkiye Hava Durumu - Cift tikla calistir
REM ===================================================
title Weather App Launcher
cd /d "%~dp0"

echo.
echo   Uygulama baslatiliyor...
echo.

REM PowerShell script'ini calistir (ExecutionPolicy bypass)
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\start.ps1"

echo.
echo   Baslatici tamamlandi. Sunucular ayri pencerelerde calisiyor.
ping -n 4 127.0.0.1 >nul
