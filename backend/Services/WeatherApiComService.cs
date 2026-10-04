using System.Net.Http.Json;
using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Services;

/// <summary>
/// WeatherAPI.com (forecast.json) istemcisi.
/// Dokümantasyon: https://www.weatherapi.com/docs/
/// </summary>
public class WeatherApiComService : IWeatherService
{
    // WeatherAPI.com snake_case alan adları kullanır (ör. maxtemp_c, daily_chance_of_rain)
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    private readonly HttpClient _http;
    private readonly ILogger<WeatherApiComService> _logger;
    private readonly string _apiKey = "";

    public WeatherApiComService(HttpClient http, IConfiguration config, ILogger<WeatherApiComService> logger)
    {
        _http = http;
        _logger = logger;
        _apiKey = ResolveApiKey(config);
    }

    /// <summary>API anahtarını sırasıyla: env var → weatherapi.key dosyası üzerinden okur.</summary>
    private static string ResolveApiKey(IConfiguration config)
    {
        // 1) Ortam değişkeni (process / User / Machine)
        var key = config["WEATHERAPI_KEY"] ?? Environment.GetEnvironmentVariable("WEATHERAPI_KEY");
        if (!string.IsNullOrWhiteSpace(key)) return key.Trim();

        // 2) weatherapi.key dosyası — birkaç aday konumda ara
        var candidates = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "weatherapi.key"),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "weatherapi.key"),
            Path.Combine(AppContext.BaseDirectory, "weatherapi.key"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "weatherapi.key"),
        };

        foreach (var candidate in candidates)
        {
            try
            {
                var full = Path.GetFullPath(candidate);
                if (!File.Exists(full)) continue;
                var content = File.ReadAllText(full).Trim();
                if (string.IsNullOrWhiteSpace(content)) continue;
                if (content.Contains("BURAYA")) continue; // placeholder, geç
                return content;
            }
            catch { /* aday yol geçersizse geç */ }
        }

        return "";
    }

    public async Task<CityForecastDto?> GetForecastAsync(City city, int days, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            _logger.LogError("WEATHERAPI_KEY tanımlı değil! Komut: setx WEATHERAPI_KEY \"ANAHTAR\"");
            throw new InvalidOperationException(
                "WEATHERAPI_KEY tanımlı değil. PowerShell'de 'setx WEATHERAPI_KEY \"SENIN_ANAHTARIN\"' çalıştırıp uygulamayı yeniden başlatın.");
        }

        days = Math.Clamp(days, 1, 7);

        // GET /v1/forecast.json?key=API_KEY&q=LAT,LON&days=N&lang=TR&aqi=no&alerts=no
        // NOT: Koordinatlar INVARIANT culture ile formatlanmalı (ondalık ayracı nokta olmalı),
        // aksi halde tr-TR kültüründe "39,9334" yazılır ve API yanlış konum döndürür.
        var lat = city.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var lon = city.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

        var url = $"v1/forecast.json" +
                  $"?key={_apiKey}" +
                  $"&q={lat},{lon}" +
                  $"&days={days}" +
                  "&lang=TR" +
                  "&aqi=no" +
                  "&alerts=no";

        using var response = await _http.GetAsync(url, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("WeatherAPI.com hatası ({Status}) {City}: {Body}", response.StatusCode, city.Name, body);
            return null;
        }

        var data = JsonSerializer.Deserialize<WeatherApiResponse>(body, JsonOpts);
        if (data?.Error is not null)
        {
            _logger.LogWarning("WeatherAPI.com hata yanıtı {City}: [{Code}] {Message}", city.Name, data.Error.Code, data.Error.Message);
            return null;
        }
        if (data?.Forecast?.Forecastday is not { Count: > 0 })
            return null;

        var dayDtos = data.Forecast.Forecastday
            .Take(days)
            .Select(d => new DailyForecastDto(
                Date: DateOnly.Parse(d.Date),
                ConditionText: d.Day.Condition.Text ?? "-",
                IconUrl: NormalizeIcon(d.Day.Condition.Icon),
                TempMinC: d.Day.MintempC,
                TempMaxC: d.Day.MaxtempC,
                TempAvgC: d.Day.AvgtempC,
                HumidityPercent: d.Day.Avghumidity,
                PrecipMm: d.Day.TotalprecipMm,
                PrecipChancePercent: d.Day.DailyChanceOfRain,
                WillItRain: d.Day.DailyWillItRain == 1,
                SnowChancePercent: d.Day.DailyChanceOfSnow,
                WillItSnow: d.Day.DailyWillItSnow == 1,
                WindMaxKph: d.Day.MaxwindKph,
                UvIndex: d.Day.Uv
            ))
            .ToList();

        return new CityForecastDto(
            City: city.Name,
            Region: data.Location.Region,
            Latitude: data.Location.Lat,
            Longitude: data.Location.Lon,
            Timezone: data.Location.TzId,
            Days: dayDtos
        );
    }

    /// <summary>WeatherAPI.com ikon yolları "//cdn.weatherapi.com/..." şeklinde gelir; https ekler.</summary>
    private static string? NormalizeIcon(string? icon) =>
        string.IsNullOrWhiteSpace(icon) ? null :
        icon.StartsWith("//") ? "https:" + icon : icon;
}