using Microsoft.Extensions.Logging;
using WeatherAgent.Domain.Common;
using WeatherAgent.Infrastructure.ConciergeAgent;
using WeatherAgent.Infrastructure.Geolocation;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.Application.WeatherSuggestion
{
    public class WeatherSuggestionCommand(
        IGeolocationService geolocationService,
        IConciergeAgentService conciergeAgentService,
        IWeatherService weatherService,
        ILogger<WeatherSuggestionCommand> logger) : IWeatherSuggestionCommand
    {
        public async Task<Result<string>> ExecuteAsync(string location)
        {
            logger.LogInformation("Starting weather suggestion for location: {Location}", location);
            
            if (string.IsNullOrWhiteSpace(location))
            {
                logger.LogWarning("Location parameter is null or empty");
                return Result.Failure<string>(ErrorMessages.LocationRequired);
            }

            var geolocationResult = await geolocationService.GetCoordinatesAsync(location);

            if (geolocationResult.IsFailure)
            {
                logger.LogWarning("Failed to get coordinates for location: {Location}. Error: {Error}", location, geolocationResult.Error);
                return Result.Failure<string>(geolocationResult.Error);
            }

            logger.LogInformation("Coordinates found for {Location}: Lat={Latitude}, Lon={Longitude}", 
                location, geolocationResult.Value.Latitude, geolocationResult.Value.Longitude);

            var weatherResult = await weatherService.GetCurrentWeatherAsync(geolocationResult.Value.Latitude, geolocationResult.Value.Longitude);

            if (weatherResult.IsFailure)
            {
                logger.LogWarning("Failed to get weather data for location: {Location}. Error: {Error}", location, weatherResult.Error);
                return Result.Failure<string>(weatherResult.Error);
            }

            var weather = weatherResult.Value;
            logger.LogInformation("Weather data retrieved - Temperature: {Temperature}°C, Apparent: {ApparentTemp}°C, Precipitation: {Precipitation}%",
                weather.Current.Temperature2m, weather.Current.ApparentTemperature, weather.Current.PrecipitationProbability);

            var userInput = $"City: {location}\n" +
                            $"Temperature: {weather.Current.Temperature2m} °C\n" +
                            $"Apparent Temperature: {weather.Current.ApparentTemperature} °C\n" +
                            $"Precipitation Probability: {weather.Current.PrecipitationProbability} %";

            var suggestionResult = await conciergeAgentService.GetConciergeResponseAsync(userInput);

            if (suggestionResult.IsFailure)
            {
                logger.LogWarning("Failed to get AI suggestion for location: {Location}. Error: {Error}", location, suggestionResult.Error);
                return Result.Failure<string>(suggestionResult.Error);
            }

            logger.LogInformation("Weather suggestion completed successfully for location: {Location}", location);

            return Result.Success(suggestionResult.Value);
        }
    }
}
