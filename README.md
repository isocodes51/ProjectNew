# 🌤️ Türkiye Hava Durumu (7 Günlük Tahmin)

**.NET 10 Web API + React (Vite)** ile seçtiğiniz Türkiye illerinin önümüzdeki 7 günlük hava durumu tahminini gösteren uygulama. Veri kaynağı: **Google Maps Platform Weather API**.

## 📁 Proje Yapısı

ProjectNew/
├── start.bat                    # 🖱️ Çift tıkla başlat (masaüstü kısayolu)
├── stop.bat                     # 🛑 Çift tıkla durdur
├── scripts/
│   ├── start.ps1                # Backend + Frontend başlatıcı
│   ├── stop.ps1                 # Sunucu durdurucu
│   └── create-desktop-shortcut.ps1  # Masaüstü kısayolu üretir
├── backend/                     # ASP.NET Core Web API (.NET 10)

## 🖱️ Çift Tıklama ile Çalıştırma (Önerilen)

Masaüstünüzdeki kısayollardan veya proje kökündeki `.bat` dosyalarından **çift tıklayarak** uygulamayı başlatabilirsiniz:

| Dosya | Görev |
|---|---|
| `start.bat` | Backend + Frontend'i başlatır, tarayıcıyı otomatik açar |
| `stop.bat` | Tüm sunucuları durdurur |
| `scripts/create-desktop-shortcut.ps1` | Masaüstüne kısayol oluşturur (sağ tık → Run with PowerShell) |

**İlk kurulum:**
1. `WEATHERAPI_KEY` ortam değişkenini ayarlayın (aşağıya bakın)
2. `scripts/create-desktop-shortcut.ps1`'i çalıştırın → masaüstüne **"Hava Durumu"** kısayolu gelir
3. Bundan sonra sadece **çift tıklayın** — sunucular açılır, tarayıcı otomatik gelir!

> 💡 `start.bat` ilk çalıştırmada `node_modules` yoksa otomatik `npm install` yapar.

## 🚀 Manuel Çalıştırma

### 1. WeatherAPI.com Anahtarı

[WeatherAPI.com](https://www.weatherapi.com/signup.aspx) üzerinden ücretsiz hesap oluşturup API anahtarınızı alın.

Anahtarı ortam değişkeni olarak tanımlayın (PowerShell):

Örnek yanıt (`GET /api/weather/Ankara`):

## 🛠️ Teknik Notlar

- **Endpoint**: `GET https://api.weatherapi.com/v1/forecast.json?key=...&q=LAT,LON&days=N&lang=TR`
- **Birimler**: METRIC (°C, km/s, mm) — WeatherAPI.com varsayılanı
- **Dil**: `lang=TR` sayesinde hava durumu açıklamaları Türkçe gelir
- **Kar bilgisi**: `dailyChanceOfSnow` / `dailyWillItSnow` alanları kar uyarıları için
- **UV indeksi**: `day.uv` (0-11+ arası skala)
- **Hata yönetimi**: Bulunamayan il → `404`, dış API hatası → `502`
- **CORS**: `http://localhost:5173` origin'ine izin verilir
- **Rate limit**: Ücretsiz planda ayda 1M çağrı / ~60 çağrı/dk sınırı vardır
- **Güvenlik uyarısı**: Derlemede `Microsoft.OpenApi 2.0.0` için NU1903 uyarısı görünür; `dotnet add package Microsoft.OpenApi` ile sürümlü paketi güncelleyebilirsiniz.

