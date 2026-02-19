using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.OpenAI;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using WeatherAgent.Domain.Configuration;
using WeatherAgent.Infrastructure.ConciergeAgent.Constants;

namespace WeatherAgent.Infrastructure.ConciergeAgent
{
    public class ConciergeAgentService : IConciergeAgentService
    {
        private readonly AIAgent _agent;
        private readonly AIConfiguration _aiConfig;
        private readonly ILogger<ConciergeAgentService> _logger;

        public ConciergeAgentService(AIConfiguration aiConfig, ILogger<ConciergeAgentService> logger)
        {
            _aiConfig = aiConfig;
            _logger = logger;

            _logger.LogInformation("Initializing ConciergeAgentService with BaseUrl: {BaseUrl}", aiConfig.BaseUrl);

            var client = string.IsNullOrEmpty(aiConfig.ApiKey)
                ? new AzureOpenAIClient(new Uri(aiConfig.BaseUrl!), new DefaultAzureCredential())
                : new AzureOpenAIClient(new Uri(aiConfig.BaseUrl!), new AzureKeyCredential(aiConfig.ApiKey));

            _agent = client
                .GetChatClient("gpt-4o-mini")
                .AsAIAgent(instructions: AgentConstants.AgentPrompt);

            _logger.LogInformation("ConciergeAgentService initialized successfully");
        }

        public async Task<string> GetConciergeResponseAsync(string userInput)
        {
            _logger.LogInformation("Getting concierge response for input: {UserInput}", userInput);
            
            try
            {
                var response = await _agent.RunAsync(userInput);
                
                _logger.LogInformation("Concierge response received successfully. Length: {Length} characters", response.Text?.Length ?? 0);
                
                return response.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting concierge response for input: {UserInput}", userInput);
                throw;
            }
        }
    }
}
