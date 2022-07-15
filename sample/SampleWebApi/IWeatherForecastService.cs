namespace SampleWebApi;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> List();
}