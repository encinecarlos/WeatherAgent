using WeatherAgent.Domain.Common;

namespace WeatherAgent.Infrastructure.Geolocation
{
    public interface IGeolocationService
    {
        Task<Result<(double Latitude, double Longitude)>> GetCoordinatesAsync(string location);
    }
}
