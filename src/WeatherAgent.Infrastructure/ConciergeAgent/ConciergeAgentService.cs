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

            _logger.LogDebug("Initializing ConciergeAgentService with BaseUrl: {BaseUrl}", aiConfig.BaseUrl);

            var client = string.IsNullOrEmpty(aiConfig.ApiKey)
                ? new AzureOpenAIClient(new Uri(aiConfig.BaseUrl!), new DefaultAzureCredential())
                : new AzureOpenAIClient(new Uri(aiConfig.BaseUrl!), new AzureKeyCredential(aiConfig.ApiKey));

            _agent = client
                .GetChatClient("gpt-4o-mini")
                .AsAIAgent(instructions: AgentConstants.AgentPrompt);

            _logger.LogDebug("ConciergeAgentService initialized successfully");
        }

        public async Task<string> GetConciergeResponseAsync(string userInput)
        {
            var inputLength = userInput?.Length ?? 0;
            var inputPreview = userInput is { Length: > 100 }
                ? userInput[..100] + "..."
                : userInput ?? string.Empty;

            _logger.LogInformation(
                "Getting concierge response for input. Length: {UserInputLength} characters. Preview: {UserInputPreview}",
                inputLength,
                inputPreview);

            var response = await _agent.RunAsync(userInput);

            _logger.LogInformation("Concierge response received successfully. Length: {Length} characters", response.Text?.Length ?? 0);

            return response.Text ?? string.Empty;
        }
    }
}
