using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    /// <summary>Türkiye'deki 81 ili listeler.</summary>
    [HttpGet("cities")]
    public ActionResult<IReadOnlyList<City>> GetCities() => Ok(TurkishCities.All);

    /// <summary>Seçilen il için önümüzdeki N (varsayılan 7) günlük tahmini getirir.</summary>
    [HttpGet("{city}")]
    public async Task<ActionResult<CityForecastDto>> GetForecast(string city, [FromQuery] int days = 7, CancellationToken ct = default)
    {
        var found = TurkishCities.Find(city);
        if (found is null)
            return NotFound(new { message = $"'{city}' bulunamadı. Desteklenen iller için /api/weather/cities adresine bakın." });

        try
        {
            var forecast = await weatherService.GetForecastAsync(found, days, ct);
            if (forecast is null)
                return StatusCode(StatusCodes.Status502BadGateway,
                    new { message = "WeatherAPI.com'dan veri alınamadı. API anahtarınızı ve internet bağlantınızı kontrol edin." });

        return Ok(forecast);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }

    /// <summary>Aynı anda birden fazla ilin tahminini getirir (virgülle ayırın): Ankara,İzmir,Bursa</summary>
    [HttpGet("batch")]
    public async Task<ActionResult<List<CityForecastDto>>> GetBatch([FromQuery] string cities, [FromQuery] int days = 7, CancellationToken ct = default)
    {
        var names = cities.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (names.Length == 0)
            return BadRequest(new { message = "'cities' parametresi boş olamaz." });
        if (names.Length > 10)
            return BadRequest(new { message = "Tek istekte en fazla 10 il sorgulanabilir." });

        try
        {
            var results = new List<CityForecastDto>();
            var errors = new List<string>();

            foreach (var name in names)
            {
                var city = TurkishCities.Find(name);
                if (city is null)
                {
                    errors.Add($"'{name}' listede yok.");
                    continue;
                }

                var forecast = await weatherService.GetForecastAsync(city, days, ct);
                if (forecast is not null) results.Add(forecast);
                else errors.Add($"'{name}' için veri alınamadı.");
            }

            return Ok(new { results, errors });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}
