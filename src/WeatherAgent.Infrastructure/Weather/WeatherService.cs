

using System.Globalization;
using Microsoft.Extensions.Logging;
using RestSharp;
using WeatherAgent.Domain.Common;
using WeatherAgent.Domain.Configuration;

namespace WeatherAgent.Infrastructure.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IRestClient _restClient;
        private readonly WeatherConfiguration _weatherConfiguration;
        private readonly ILogger<WeatherService> _logger;
        
        public WeatherService(WeatherConfiguration weatherConfiguration, ILogger<WeatherService> logger)
        {
            _weatherConfiguration = weatherConfiguration;
            _logger = logger;
            _restClient = new RestClient(_weatherConfiguration.BaseUrl);
            
            _logger.LogInformation("WeatherService initialized with URL: {BaseUrl}", _weatherConfiguration.BaseUrl);
        }

        public async Task<Result<Domain.Entities.Weather>> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            _logger.LogInformation("Requesting weather data for Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
            
            try
            {
                var request = new RestRequest("/forecast", Method.Get);
                request
                    .AddQueryParameter("latitude", latitude.ToString(CultureInfo.InvariantCulture))
                    .AddQueryParameter("longitude", longitude.ToString(CultureInfo.InvariantCulture))
                    .AddQueryParameter("current", "temperature_2m,apparent_temperature,precipitation_probability")
                    .AddQueryParameter("timezone", "auto");

                var response = await _restClient.ExecuteAsync<Domain.Entities.Weather>(request);
                
                if (!response.IsSuccessful)
                {
                    _logger.LogWarning("Weather request failed for Lat={Latitude}, Lon={Longitude}. StatusCode: {StatusCode}",
                        latitude, longitude, response.StatusCode);
                    return Result.Failure<Domain.Entities.Weather>(ErrorMessages.WeatherServiceError);
                }
                
                if (response.Data == null)
                {
                    _logger.LogWarning("Weather data is null for Lat={Latitude}, Lon={Longitude}", latitude, longitude);
                    return Result.Failure<Domain.Entities.Weather>(ErrorMessages.WeatherDataNotFound);
                }

                _logger.LogInformation("Weather data retrieved successfully for Lat={Latitude}, Lon={Longitude}", latitude, longitude);

                return Result.Success(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weather data for Lat={Latitude}, Lon={Longitude}", latitude, longitude);
                return Result.Failure<Domain.Entities.Weather>(ErrorMessages.WeatherServiceError);
            }
        }
    }
}
