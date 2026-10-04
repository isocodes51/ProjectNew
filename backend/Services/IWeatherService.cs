using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherService
{
    /// <summary>Şehir için önümüzdeki N günlük tahmini getirir.</summary>
    Task<CityForecastDto?> GetForecastAsync(City city, int days, CancellationToken ct = default);
}