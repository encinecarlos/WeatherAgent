using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using WeatherAgent.Domain.Common;
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

        public async Task<Result<string>> GetConciergeResponseAsync(string userInput)
        {
            _logger.LogInformation("Getting concierge response for input: {UserInput}", userInput);

            if (string.IsNullOrWhiteSpace(userInput))
            {
                _logger.LogWarning("User input is empty or null");
                return Result.Failure<string>(ErrorMessages.UnexpectedError);
            }

            try
            {
                var response = await _agent.RunAsync(userInput);

                if (string.IsNullOrWhiteSpace(response.Text))
                {
                    _logger.LogWarning("AI agent returned empty response");
                    return Result.Failure<string>(ErrorMessages.AiAgentResponseEmpty);
                }

                _logger.LogInformation("Concierge response received successfully. Length: {Length} characters", response.Text.Length);

                return Result.Success(response.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting concierge response for input: {UserInput}", userInput);
                return Result.Failure<string>(ErrorMessages.AiAgentError);
            }
        }
    }
}
