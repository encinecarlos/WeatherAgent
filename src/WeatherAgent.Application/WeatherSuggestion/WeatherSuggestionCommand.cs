using Microsoft.Extensions.Logging;
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
        public async Task<string> ExecuteAsync(string location)
        {
            logger.LogInformation("Starting weather suggestion for location: {Location}", location);

            var geolocationResult = await geolocationService.GetCoordinatesAsync(location);

            if (geolocationResult == null)
            {
                logger.LogWarning("Location not found: {Location}", location);
                return $"Sorry, I couldn't find the location '{location}'. Please try a different city.";
            }

            logger.LogInformation("Coordinates found for {Location}: Lat={Latitude}, Lon={Longitude}", 
                location, geolocationResult.Value.Latitude, geolocationResult.Value.Longitude);

            var weather = await weatherService.GetCurrentWeatherAsync(geolocationResult.Value.Latitude, geolocationResult.Value.Longitude);

            logger.LogInformation("Weather data retrieved - Temperature: {Temperature}°C, Apparent: {ApparentTemp}°C, Precipitation: {Precipitation}%",
                weather.Current.Temperature2m, weather.Current.ApparentTemperature, weather.Current.PrecipitationProbability);

            var userInput = $"City: {location}\n" +
                            $"Temperature: {weather.Current.Temperature2m} °C\n" +
                            $"Apparent Temperature: {weather.Current.ApparentTemperature} °C\n" +
                            $"Precipitation Probability: {weather.Current.PrecipitationProbability} %";

            var suggestion = await conciergeAgentService.GetConciergeResponseAsync(userInput);

            logger.LogInformation("Weather suggestion completed successfully for location: {Location}", location);

            return suggestion;
        }
    }
}
