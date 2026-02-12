namespace WeatherAgent.Application.WeatherSuggestion
{
    public interface IWeatherSuggestionCommand
    {
        Task<string> ExecuteAsync(string location);
    }
}
