namespace WeatherAgent.Web.Services;

public interface IWeatherAgentService
{
    Task<WeatherAgentResponse> GetWeatherSuggestionAsync(string location);
}

public record WeatherAgentResponse(string? AgentResponse, bool IsSuccess, string? ErrorMessage);
