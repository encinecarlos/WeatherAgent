using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.OpenAI;
using OpenAI.Chat;
using WeatherAgent.Domain.Configuration;
using WeatherAgent.Infrastructure.ConciergeAgent.Constants;

namespace WeatherAgent.Infrastructure.ConciergeAgent
{
    public class ConciergeAgentService : IConciergeAgentService
    {
        private readonly AIAgent _agent;
        private readonly AIConfiguration _aiConfig;

        public ConciergeAgentService(AIConfiguration aiConfig)
        {
            _aiConfig = aiConfig;

            var client = string.IsNullOrEmpty(aiConfig.ApiKey)
                ? new AzureOpenAIClient(new Uri(aiConfig.BaseUrl!), new DefaultAzureCredential())
                : new AzureOpenAIClient(new Uri(aiConfig.BaseUrl!), new AzureKeyCredential(aiConfig.ApiKey));

            _agent = client
                .GetChatClient("gpt-4o-mini")
                .AsAIAgent(instructions: AgentConstants.AgentPrompt);
        }

        public async Task<string> GetConciergeResponseAsync(string userInput)
        {
            var response = await _agent.RunAsync(userInput);

            return response.Text ?? string.Empty;
        }
    }
}
