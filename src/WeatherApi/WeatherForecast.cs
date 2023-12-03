namespace WeatherApi;

public class WeatherForecast(DateOnly date, int temperatureC, string? summary)
{
    public int TemperatureC { get; } = temperatureC;
    
    public DateOnly Date { get; } = date;
    
    public string? Summary { get; } = summary;
    
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


// public class WeatherForecast
// {
//     public DateOnly Date { get; set; }
//
//     public int TemperatureC { get; set; }
//
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//
//     public string? Summary { get; set; }
// }
