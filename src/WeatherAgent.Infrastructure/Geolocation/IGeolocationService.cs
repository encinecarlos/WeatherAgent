namespace WeatherAgent.Infrastructure.Geolocation
{
    public interface IGeolocationService
    {
        Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string location);
    }
}
