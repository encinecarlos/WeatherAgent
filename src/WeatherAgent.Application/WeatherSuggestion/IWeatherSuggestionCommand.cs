using WeatherAgent.Domain.Common;

namespace WeatherAgent.Application.WeatherSuggestion
{
    public interface IWeatherSuggestionCommand
    {
        Task<Result<string>> ExecuteAsync(string location);
    }
}
