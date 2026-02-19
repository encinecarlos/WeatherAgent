using Microsoft.Extensions.Logging;
using RestSharp;
using WeatherAgent.Domain.Configuration;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.Infrastructure.Geolocation
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IRestClient _restClient;
        private readonly WeatherConfiguration _weatherConfiguration;
        private readonly ILogger<GeolocationService> _logger;

        public GeolocationService(WeatherConfiguration weatherConfiguration, ILogger<GeolocationService> logger)
        {
            _weatherConfiguration = weatherConfiguration;
            _logger = logger;
            _restClient = new RestClient(_weatherConfiguration.GeocodingUrl);
            
            _logger.LogInformation("GeolocationService initialized with URL: {GeocodingUrl}", _weatherConfiguration.GeocodingUrl);
        }

        public async Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string location)
        {
            _logger.LogInformation("Requesting coordinates for location: {Location}", location);
            
            try
            {
                var request = new RestRequest("/search", Method.Get);
                request
                    .AddQueryParameter("name", location)
                    .AddQueryParameter("count", 10)
                    .AddQueryParameter("language", "pt")
                    .AddQueryParameter("format", "json");

                var result = await _restClient.ExecuteAsync<GeocodingResponse>(request);
                
                if (!result.IsSuccessful)
                {
                    _logger.LogWarning("Geocoding request failed for location: {Location}. StatusCode: {StatusCode}", 
                        location, result.StatusCode);
                    return null;
                }

                var firstResult = result.Data?.Results?.FirstOrDefault();
                
                if (firstResult == null)
                {
                    _logger.LogWarning("No coordinates found for location: {Location}", location);
                    return null;
                }

                _logger.LogInformation("Coordinates found for {Location}: Latitude={Latitude}, Longitude={Longitude}",
                    location, firstResult.Latitude, firstResult.Longitude);

                return (firstResult.Latitude, firstResult.Longitude);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting coordinates for location: {Location}", location);
                throw;
            }
        }
    }
}
