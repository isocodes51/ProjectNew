# ============================================================
#  Türkiye Hava Durumu - Otomatik Başlatıcı
#  Backend (.NET Web API) + Frontend (React/Vite) başlatır
# ============================================================

$ErrorActionPreference = "Continue"
$root = Split-Path -Parent $PSScriptRoot
$backendDir = Join-Path $root "backend"
$frontendDir = Join-Path $root "frontend"

Write-Host ""
Write-Host "  ================================================" -ForegroundColor Cyan
Write-Host "   Turkiye Hava Durumu - Baslatiliyor..." -ForegroundColor Cyan
Write-Host "  ================================================" -ForegroundColor Cyan
Write-Host ""

# --- 1) API anahtari cozumleme (User env -> weatherapi.key dosyasi) ---
$apiKey = [Environment]::GetEnvironmentVariable("WEATHERAPI_KEY", "User")
if ([string]::IsNullOrWhiteSpace($apiKey)) {
    $apiKey = [Environment]::GetEnvironmentVariable("WEATHERAPI_KEY", "Machine")
}
if ([string]::IsNullOrWhiteSpace($apiKey)) {
    $keyFile = Join-Path $root "weatherapi.key"
    if (Test-Path $keyFile) {
        $apiKey = (Get-Content $keyFile -Raw).Trim()
    }
}

if ([string]::IsNullOrWhiteSpace($apiKey) -or $apiKey -like "*BURAYA*") {
    Write-Host "  [UYARI] WEATHERAPI_KEY bulunamadi veya placeholder!" -ForegroundColor Yellow
    Write-Host "  Cozum 1: PowerShell'de calistirin:" -ForegroundColor Yellow
    Write-Host "      setx WEATHERAPI_KEY `"SENIN_ANAHTARIN`"" -ForegroundColor White
    Write-Host "  Cozum 2: Proje kokunde 'weatherapi.key' dosyasi olusturup" -ForegroundColor Yellow
    Write-Host "           icine sadece anahtari yazin." -ForegroundColor White
    Write-Host ""
    Write-Host "  Simdi devam ediliyor (veri cekmek icin anahtar gerekli)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 3
} else {
    # Backend surecinin dogrudan gormesi icin surec seviyesine ata
    $env:WEATHERAPI_KEY = $apiKey
    Write-Host "  [OK] API anahtari bulundu ve backend'e enjekte edildi." -ForegroundColor Green
}

# --- 2) Frontend bagimliliklari ---
if (-not (Test-Path (Join-Path $frontendDir "node_modules"))) {
    Write-Host "  [INFO] Frontend bagimliliklari yukleniyor (ilk calistirma)..." -ForegroundColor Yellow
    Push-Location $frontendDir
    npm install | Out-Null
    Pop-Location
    Write-Host "  [OK] Bagimliliklar yuklendi." -ForegroundColor Green
}

# --- 3) Backend'i yeni pencerede baslat ---
Write-Host "  [INFO] Backend baslatiliyor (https://localhost:7113)..." -ForegroundColor Cyan
Start-Process -FilePath "powershell.exe" `
    -ArgumentList "-NoExit","-Command","cd '$backendDir'; `$host.UI.RawUI.WindowTitle='Weather API - Backend'; dotnet run" `
    -WorkingDirectory $backendDir

# --- 4) Frontend'i yeni pencerede baslat ---
Write-Host "  [INFO] Frontend baslatiliyor (http://localhost:5173)..." -ForegroundColor Cyan
Start-Process -FilePath "powershell.exe" `
    -ArgumentList "-NoExit","-Command","cd '$frontendDir'; `$host.UI.RawUI.WindowTitle='Weather UI - Frontend'; npm run dev" `
    -WorkingDirectory $frontendDir

# --- 5) Sunucularin hazir olmasini bekle ve tarayiciyi ac ---
Write-Host "  [INFO] Sunucular hazirlaniyor..." -ForegroundColor Cyan
$maxWait = 60
$waited = 0
while ($waited -lt $maxWait) {
    Start-Sleep -Seconds 2
    $waited += 2
    try {
        $r = Invoke-WebRequest -Uri "http://localhost:5173" -UseBasicParsing -TimeoutSec 2
        if ($r.StatusCode -eq 200) { break }
    } catch { }
}

Write-Host ""
Write-Host "  [OK] Uygulama hazir! Tarayici aciliyor..." -ForegroundColor Green
Start-Process "http://localhost:5173"

Write-Host ""
Write-Host "  ================================================" -ForegroundColor Cyan
Write-Host "   Uygulama calisiyor." -ForegroundColor Green
Write-Host "   Durdurmak icin: stop.bat" -ForegroundColor Yellow
Write-Host "  ================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "  Bu pencereyi kapatabilirsiniz (sunucular ayri pencerelerde)." -ForegroundColor Gray