namespace WeatherAgent.Infrastructure.ConciergeAgent
{
    public interface IConciergeAgentService
    {
        Task<string> GetConciergeResponseAsync(string userInput);
    }
}
