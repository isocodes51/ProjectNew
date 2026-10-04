using WeatherApi.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS: React dev server'a izin ver
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// WeatherAPI.com API anahtarı, WeatherApiComService içinde IConfiguration üzerinden okunur.
// appsettings.json'a yazmak yerine: setx WEATHERAPI_KEY "ANAHTAR"
builder.Services.AddHttpClient<IWeatherService, WeatherApiComService>(client =>
{
    client.BaseAddress = new Uri("https://api.weatherapi.com/");
    client.Timeout = TimeSpan.FromSeconds(15);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();


