using System.Text.Json.Serialization;

namespace WeatherAgent.Application.DTO
{
    public record AgentResponseDto([property: JsonPropertyName("agentResponse")] string? AgentResponse);
}
