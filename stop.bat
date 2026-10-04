@echo off
REM ===================================================
REM  Turkiye Hava Durumu - Uygulamayi durdur
REM ===================================================
title Weather App Stopper
cd /d "%~dp0"

echo.
echo   Backend ve Frontend durduruluyor...
echo.

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\stop.ps1"

echo.
echo   Tamamlandi.
ping -n 3 127.0.0.1 >nul
