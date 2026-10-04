namespace WeatherApi.Models;

/// <summary>Frontend'e döneceğimiz sadeleştirilmiş günlük tahmin.</summary>
public record DailyForecastDto(
    DateOnly Date,
    string ConditionText,
    string? IconUrl,
    double TempMinC,
    double TempMaxC,
    double TempAvgC,
    double HumidityPercent,
    double PrecipMm,
    int PrecipChancePercent,
    bool WillItRain,
    int SnowChancePercent,
    bool WillItSnow,
    double WindMaxKph,
    double UvIndex
);

/// <summary>Bir şehrin 7 günlük tahmini.</summary>
public record CityForecastDto(
    string City,
    string? Region,
    double Latitude,
    double Longitude,
    string? Timezone,
    IReadOnlyList<DailyForecastDto> Days
);