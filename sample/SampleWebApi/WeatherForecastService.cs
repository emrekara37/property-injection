using DotnetPropertyInjection.Lib;

namespace SampleWebApi;

public class WeatherForecastService : IWeatherForecastService
{
    [Inject]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    [Inject]
    public ILogger<WeatherForecast> Logger { get; set; }
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public IEnumerable<WeatherForecast> List()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}