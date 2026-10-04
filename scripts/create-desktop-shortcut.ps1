# ============================================================
#  Masaustune "Hava Durumu" kisayolu olusturur
#  Calistirmak icin: sag tikla -> "Run with PowerShell"
# ============================================================

$desktop = [Environment]::GetFolderPath("Desktop")
$root = Split-Path -Parent $PSScriptRoot
$startBat = Join-Path $root "start.bat"
$stopBat = Join-Path $root "stop.bat"

$WshShell = New-Object -ComObject WScript.Shell

# --- Ana kisayol: Uygulamayi Baslat ---
$shortcut = $WshShell.CreateShortcut((Join-Path $desktop "Hava Durumu.lnk"))
$shortcut.TargetPath = $startBat
$shortcut.WorkingDirectory = $root
$shortcut.WindowStyle = 1
$shortcut.Description = "Turkiye 7 Gunluk Hava Durumu - Baslat"
$shortcut.Save()

# --- Yardimci kisayol: Uygulamayi Durdur ---
$shortcut2 = $WshShell.CreateShortcut((Join-Path $desktop "Hava Durumu - Durdur.lnk"))
$shortcut2.TargetPath = $stopBat
$shortcut2.WorkingDirectory = $root
$shortcut2.WindowStyle = 1
$shortcut2.Description = "Turkiye 7 Gunluk Hava Durumu - Durdur"
$shortcut2.Save()

Write-Host ""
Write-Host "  [OK] Masaustune kisayollar olusturuldu:" -ForegroundColor Green
Write-Host "       - Hava Durumu.lnk         (baslat)" -ForegroundColor White
Write-Host "       - Hava Durumu - Durdur.lnk (durdur)" -ForegroundColor White
Write-Host ""
Write-Host "  Masaustunden cift tiklayarak uygulamayi baslatabilirsiniz." -ForegroundColor Cyan