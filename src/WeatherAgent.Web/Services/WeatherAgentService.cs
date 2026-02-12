using System.Net.Http.Json;

namespace WeatherAgent.Web.Services;

public class WeatherAgentService : IWeatherAgentService
{
    private readonly HttpClient _httpClient;

    public WeatherAgentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherAgentResponse> GetWeatherSuggestionAsync(string location)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/concierge?location={Uri.EscapeDataString(location)}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new WeatherAgentResponse(null, false, $"Error: {response.StatusCode} - {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<ConciergeResponse>();
            return new WeatherAgentResponse(result?.AgentResponse, true, null);
        }
        catch (Exception ex)
        {
            return new WeatherAgentResponse(null, false, $"Connection error: {ex.Message}");
        }
    }

    private record ConciergeResponse(string? AgentResponse);
}
