# ============================================================
#  Türkiye Hava Durumu - Durdurucu
#  5173 (Vite) ve 7113/5062 (Kestrel) portlarını dinleyen süreçleri sonlandırır
# ============================================================

$ports = @(5173, 7113, 5062)

foreach ($port in $ports) {
    $conns = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue
    foreach ($conn in $conns) {
        $procId = $conn.OwningProcess
        $proc = Get-Process -Id $procId -ErrorAction SilentlyContinue
        if ($proc) {
            Write-Host "  [DURDUR] Port $port -> PID $procId ($($proc.ProcessName))" -ForegroundColor Yellow
            Stop-Process -Id $procId -Force -ErrorAction SilentlyContinue
        }
    }
}

Write-Host ""
Write-Host "  [OK] Tum sunucular durduruldu." -ForegroundColor Green