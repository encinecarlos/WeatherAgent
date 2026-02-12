namespace WeatherAgent.Infrastructure.Weather
{
    public interface IWeatherService
    {
        Task<Domain.Entities.Weather> GetCurrentWeatherAsync(double latitude, double longitude);
    }
}
