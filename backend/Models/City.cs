namespace WeatherApi.Models;

/// <summary>Türkiye'deki il bilgisi (Google Weather API koordinatları ile).</summary>
public record City(string Name, double Latitude, double Longitude);