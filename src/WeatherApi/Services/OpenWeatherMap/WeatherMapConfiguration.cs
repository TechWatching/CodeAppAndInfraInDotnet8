using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace WeatherApi.Services.OpenWeatherMap;

public class WeatherMapConfiguration
{
    [Required]
    public required string ApiKey { get; init; }

    [Required]
    [Url]
    public required string Uri { get; init; }
    
}

[OptionsValidator]
public partial class WeatherMapConfigurationValidator : IValidateOptions<WeatherMapConfiguration>
{
}