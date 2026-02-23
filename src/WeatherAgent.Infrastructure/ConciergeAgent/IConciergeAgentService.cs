using WeatherAgent.Domain.Common;

namespace WeatherAgent.Infrastructure.ConciergeAgent
{
    public interface IConciergeAgentService
    {
        Task<Result<string>> GetConciergeResponseAsync(string userInput);
    }
}
