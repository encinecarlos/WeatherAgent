using WeatherAgent.Domain.Configuration;

namespace WeatherAgent.Tests.Domain;

public class AIConfigurationTests
{
    [Fact]
    public void BaseUrl_SetAndGet_ReturnsCorrectValue()
    {
        var config = new AIConfiguration { BaseUrl = "https://myopenai.openai.azure.com/" };

        Assert.Equal("https://myopenai.openai.azure.com/", config.BaseUrl);
    }

    [Fact]
    public void ApiKey_SetAndGet_ReturnsCorrectValue()
    {
        var config = new AIConfiguration { ApiKey = "test-api-key" };

        Assert.Equal("test-api-key", config.ApiKey);
    }

    [Fact]
    public void DefaultValues_AreNull()
    {
        var config = new AIConfiguration();

        Assert.Null(config.BaseUrl);
        Assert.Null(config.ApiKey);
    }

    [Fact]
    public void ApiKey_WhenEmpty_IsNotNullOrEmpty()
    {
        var config = new AIConfiguration { ApiKey = "" };

        Assert.True(string.IsNullOrEmpty(config.ApiKey));
    }

    [Fact]
    public void ApiKey_WhenSet_IsNotNullOrEmpty()
    {
        var config = new AIConfiguration { ApiKey = "some-key" };

        Assert.False(string.IsNullOrEmpty(config.ApiKey));
    }
}
