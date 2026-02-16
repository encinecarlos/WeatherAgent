using WeatherAgent.Application.DTO;

namespace WeatherAgent.Tests.Application;

public class AgentResponseDtoTests
{
    [Fact]
    public void Constructor_SetsAgentResponse()
    {
        var dto = new AgentResponseDto("Hello from the agent!");

        Assert.Equal("Hello from the agent!", dto.AgentResponse);
    }

    [Fact]
    public void Constructor_WithNull_SetsNullAgentResponse()
    {
        var dto = new AgentResponseDto(null);

        Assert.Null(dto.AgentResponse);
    }

    [Fact]
    public void Constructor_WithEmptyString_SetsEmptyAgentResponse()
    {
        var dto = new AgentResponseDto(string.Empty);

        Assert.Equal(string.Empty, dto.AgentResponse);
    }

    [Fact]
    public void Equality_TwoDtosWithSameValue_AreEqual()
    {
        var dto1 = new AgentResponseDto("Same response");
        var dto2 = new AgentResponseDto("Same response");

        Assert.Equal(dto1, dto2);
    }

    [Fact]
    public void Equality_TwoDtosWithDifferentValues_AreNotEqual()
    {
        var dto1 = new AgentResponseDto("Response A");
        var dto2 = new AgentResponseDto("Response B");

        Assert.NotEqual(dto1, dto2);
    }
}
