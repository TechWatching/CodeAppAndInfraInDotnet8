using WeatherApi.Services.OpenWeatherMap;

namespace WeatherApi.Services;

using Coordinates = (double Latitude, double Longitude);

public class OpenWeatherService : IWeatherService
{
    private readonly IOpenWeatherMapApi _openWeatherMapApi;
    private static readonly Coordinates BordeauxCoordinates = (44.837789, -0.57918);

    public OpenWeatherService(IOpenWeatherMapApi openWeatherMapApi)
    {
        _openWeatherMapApi = openWeatherMapApi;
    }

    public async Task<WeatherForecast[]> GetWeatherForecasts()
    {
        var weatherApiResponse = await _openWeatherMapApi.GetWeatherForecast(BordeauxCoordinates.Latitude, BordeauxCoordinates.Longitude);
        
        var computeWeatherSummary = (double temperature, string defaultSummary = "Warm") =>
            temperature switch
            {
                < 0 => "Freezing",
                >= 0 and < 5 => "Bracing",
                >= 5 and < 12 => "Chilly",
                >= 12 and < 18 => "Cool",
                >= 18 and < 24 => "Mild",
                >= 24 and < 30 => "Warm",
                >= 30 and < 35 => "Balmy",
                >= 35 and < 40 => "Hot",
                >= 40 and < 45 => "Sweltering",
                >= 45 => "Scorching",
                _ => defaultSummary
            };
        return weatherApiResponse.List
            .Select(x =>
                new WeatherForecast(
                    date: DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(x.Dt).Date),
                    temperatureC: Convert.ToInt32(x.Main.Temp),
                    summary: computeWeatherSummary(x.Main.Temp)))
            .ToArray();
    }
}