# 🌤️ Türkiye Hava Durumu (7 Günlük Tahmin)

**.NET 10 Web API + React (Vite)** ile seçtiğiniz Türkiye illerinin önümüzdeki 7 günlük hava durumu tahminini gösteren uygulama. Veri kaynağı: **WeatherAPI.com**.

> 🐳 **Docker ile tek komutla çalıştırma** aşağıdadır!

## 📁 Proje Yapısı

ProjectNew/
├── start.bat                    # 🖱️ Çift tıkla başlat (masaüstü kısayolu)
├── stop.bat                     # 🛑 Çift tıkla durdur
├── scripts/
│   ├── start.ps1                # Backend + Frontend başlatıcı
│   ├── stop.ps1                 # Sunucu durdurucu
│   └── create-desktop-shortcut.ps1  # Masaüstü kısayolu üretir
├── backend/                     # ASP.NET Core Web API (.NET 10)

## 🐳 Docker ile Çalıştırma (Önerilen)

Tek komutla hem backend hem frontend ayağa kalkar:

