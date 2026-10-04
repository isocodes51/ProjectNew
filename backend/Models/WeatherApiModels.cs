namespace WeatherApi.Models;

/// <summary>WeatherAPI.com (forecast.json) ham yanıt modelleri.</summary>
public class WeatherApiResponse
{
    public LocationInfo Location { get; set; } = new();
    public ForecastBlock Forecast { get; set; } = new();
    public ApiError? Error { get; set; }
}

public class LocationInfo
{
    public string? Name { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string? TzId { get; set; }
}

public class ApiError
{
    public int Code { get; set; }
    public string? Message { get; set; }
}

public class ForecastBlock
{
    public List<ForecastDay> Forecastday { get; set; } = [];
}

public class ForecastDay
{
    public string Date { get; set; } = "";
    public DaySummary Day { get; set; } = new();
}

public class DaySummary
{
    public double MaxtempC { get; set; }
    public double MintempC { get; set; }
    public double AvgtempC { get; set; }
    public double MaxwindKph { get; set; }
    public double TotalprecipMm { get; set; }
    public double TotalprecipIn { get; set; }
    public double TotalsnowCm { get; set; }
    public int Avghumidity { get; set; }
    public int DailyWillItRain { get; set; }
    public int DailyChanceOfRain { get; set; }
    public int DailyWillItSnow { get; set; }
    public int DailyChanceOfSnow { get; set; }
    public double Uv { get; set; }
    public Condition Condition { get; set; } = new();
}

public class Condition
{
    public string? Text { get; set; }
    public string? Icon { get; set; }
    public int Code { get; set; }
}