namespace WeatherAgent.Domain.Common
{
    public static class ErrorMessages
    {
        // Geolocation errors
        public const string LocationNotFound = "We couldn't find the location you specified. Please try a different city name.";
        public const string GeolocationServiceError = "There was a problem finding the location. Please try again later.";
        
        // Weather errors
        public const string WeatherDataNotFound = "Weather data is currently unavailable for this location. Please try again later.";
        public const string WeatherServiceError = "There was a problem retrieving weather information. Please try again later.";
        
        // AI Agent errors
        public const string AiAgentError = "We're having trouble generating suggestions at the moment. Please try again later.";
        public const string AiAgentResponseEmpty = "No suggestion could be generated at this time. Please try again.";
        
        // Validation errors
        public const string LocationRequired = "Please enter a city name.";
        public const string LocationInvalid = "The location name provided is invalid.";
        
        // General errors
        public const string UnexpectedError = "An unexpected error occurred. Please try again.";
    }
}
