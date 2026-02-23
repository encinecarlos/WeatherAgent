using WeatherAgent.Domain.Common;

namespace WeatherAgent.Infrastructure.Weather
{
    public interface IWeatherService
    {
        Task<Result<Domain.Entities.Weather>> GetCurrentWeatherAsync(double latitude, double longitude);
    }
}
