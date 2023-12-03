using System.Web;
using Microsoft.Extensions.Options;

namespace WeatherApi.Services.OpenWeatherMap;

public class ApiKeyHandler : DelegatingHandler
{
    private readonly WeatherMapConfiguration _configuration;

    public ApiKeyHandler(IOptions<WeatherMapConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder(request.RequestUri);
        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryParams["appid"] = _configuration.ApiKey;
        uriBuilder.Query = queryParams.ToString();
        request.RequestUri = uriBuilder.Uri;
        return base.SendAsync(request, cancellationToken);
    }
}