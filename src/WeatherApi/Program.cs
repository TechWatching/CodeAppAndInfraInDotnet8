using Microsoft.Extensions.Options;
using Refit;
using WeatherApi.Services;
using WeatherApi.Services.OpenWeatherMap;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyedTransient<IWeatherService, RandomWeatherService>("random");
builder.Services.AddKeyedTransient<IWeatherService, OpenWeatherService>("api");

// Weather Map API
builder.Services.Configure<WeatherMapConfiguration>(builder.Configuration.GetSection("WeatherMap"));
builder.Services.AddSingleton<IValidateOptions<WeatherMapConfiguration>, WeatherMapConfigurationValidator>();
builder.Services.AddTransient<ApiKeyHandler>();
builder.Services.AddRefitClient<IOpenWeatherMapApi>()
    .ConfigureHttpClient((provider, client) =>
    {
        var configuration = provider.GetRequiredService<IOptions<WeatherMapConfiguration>>().Value;
        client.BaseAddress = new Uri(configuration.Uri);
    })
    .AddHttpMessageHandler<ApiKeyHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
