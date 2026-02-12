using WeatherAgent.Infrastructure.ConciergeAgent;
using WeatherAgent.Infrastructure.Geolocation;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.Application.WeatherSuggestion
{
    public class WeatherSuggestionCommand(
        IGeolocationService geolocationService,
        IConciergeAgentService conciergeAgentService,
        IWeatherService weatherService) : IWeatherSuggestionCommand
    {
        public async Task<string> ExecuteAsync(string location)
        {
            var geolocationResult = await geolocationService.GetCoordinatesAsync(location);

            if (geolocationResult == null)
            {
                return $"Sorry, I couldn't find the location '{location}'. Please try a different city.";
            }

            var weather = await weatherService.GetCurrentWeatherAsync(geolocationResult.Value.Latitude, geolocationResult.Value.Longitude);

            var userInput = $"City: {location}\n" +
                            $"Temperature: {weather.Current.Temperature2m} °C\n" +
                            $"Apparent Temperature: {weather.Current.ApparentTemperature} °C\n" +
                            $"Precipitation Probability: {weather.Current.PrecipitationProbability} %";

            var suggestion = await conciergeAgentService.GetConciergeResponseAsync(userInput);

            return suggestion;
        }
    }
}
