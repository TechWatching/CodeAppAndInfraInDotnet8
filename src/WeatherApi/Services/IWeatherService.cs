namespace WeatherApi.Services;

public interface IWeatherService
{
    Task<WeatherForecast[]> GetWeatherForecasts();
}