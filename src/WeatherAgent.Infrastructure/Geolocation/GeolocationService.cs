using Microsoft.Extensions.Logging;
using RestSharp;
using WeatherAgent.Domain.Common;
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

        public async Task<Result<(double Latitude, double Longitude)>> GetCoordinatesAsync(string location)
        {
            _logger.LogInformation("Requesting coordinates for location: {Location}", location);
            
            if (string.IsNullOrWhiteSpace(location))
            {
                _logger.LogWarning("Location is empty or null");
                return Result.Failure<(double, double)>(ErrorMessages.LocationRequired);
            }

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
                    return Result.Failure<(double, double)>(ErrorMessages.GeolocationServiceError);
                }

                var firstResult = result.Data?.Results?.FirstOrDefault();
                
                if (firstResult == null)
                {
                    _logger.LogWarning("No coordinates found for location: {Location}", location);
                    return Result.Failure<(double, double)>(ErrorMessages.LocationNotFound);
                }

                _logger.LogInformation("Coordinates found for {Location}: Latitude={Latitude}, Longitude={Longitude}",
                    location, firstResult.Latitude, firstResult.Longitude);

                return Result.Success((firstResult.Latitude, firstResult.Longitude));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting coordinates for location: {Location}", location);
                return Result.Failure<(double, double)>(ErrorMessages.GeolocationServiceError);
            }
        }
    }
}
