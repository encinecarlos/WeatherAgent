using Moq;
using WeatherAgent.Application.WeatherSuggestion;
using WeatherAgent.Domain.Entities;
using WeatherAgent.Infrastructure.ConciergeAgent;
using WeatherAgent.Infrastructure.Geolocation;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.Tests.Application;

public class WeatherSuggestionCommandTests
{
    private readonly Mock<IGeolocationService> _geolocationServiceMock;
    private readonly Mock<IConciergeAgentService> _conciergeAgentServiceMock;
    private readonly Mock<IWeatherService> _weatherServiceMock;
    private readonly WeatherSuggestionCommand _sut;

    public WeatherSuggestionCommandTests()
    {
        _geolocationServiceMock = new Mock<IGeolocationService>();
        _conciergeAgentServiceMock = new Mock<IConciergeAgentService>();
        _weatherServiceMock = new Mock<IWeatherService>();
        _sut = new WeatherSuggestionCommand(
            _geolocationServiceMock.Object,
            _conciergeAgentServiceMock.Object,
            _weatherServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenLocationNotFound_ReturnsErrorMessage()
    {
        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("UnknownCity"))
            .ReturnsAsync((ValueTuple<double, double>?)null);

        var result = await _sut.ExecuteAsync("UnknownCity");

        Assert.Equal("Sorry, I couldn't find the location 'UnknownCity'. Please try a different city.", result);
        _weatherServiceMock.Verify(x => x.GetCurrentWeatherAsync(It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        _conciergeAgentServiceMock.Verify(x => x.GetConciergeResponseAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenLocationFound_CallsWeatherServiceWithCorrectCoordinates()
    {
        var coordinates = (Latitude: 49.25, Longitude: -123.12);
        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .ReturnsAsync(coordinates);
        _weatherServiceMock
            .Setup(x => x.GetCurrentWeatherAsync(49.25, -123.12))
            .ReturnsAsync(CreateWeatherData());
        _conciergeAgentServiceMock
            .Setup(x => x.GetConciergeResponseAsync(It.IsAny<string>()))
            .ReturnsAsync("Wear a jacket!");

        await _sut.ExecuteAsync("Vancouver");

        _weatherServiceMock.Verify(x => x.GetCurrentWeatherAsync(49.25, -123.12), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenLocationFound_PassesFormattedWeatherDataToAgent()
    {
        var coordinates = (Latitude: 49.25, Longitude: -123.12);
        string? capturedInput = null;

        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .ReturnsAsync(coordinates);
        _weatherServiceMock
            .Setup(x => x.GetCurrentWeatherAsync(49.25, -123.12))
            .ReturnsAsync(CreateWeatherData(temperature: 15.5f, apparentTemp: 13.2f, precipitationProb: 20));
        _conciergeAgentServiceMock
            .Setup(x => x.GetConciergeResponseAsync(It.IsAny<string>()))
            .Callback<string>(input => capturedInput = input)
            .ReturnsAsync("Enjoy the weather!");

        await _sut.ExecuteAsync("Vancouver");

        Assert.NotNull(capturedInput);
        Assert.Contains("City: Vancouver", capturedInput);
        Assert.Contains($"Temperature: {15.5f} °C", capturedInput);
        Assert.Contains($"Apparent Temperature: {13.2f} °C", capturedInput);
        Assert.Contains("Precipitation Probability: 20 %", capturedInput);
    }

    [Fact]
    public async Task ExecuteAsync_WhenAllServicesSucceed_ReturnsConciergeResponse()
    {
        var coordinates = (Latitude: 49.25, Longitude: -123.12);
        var expectedSuggestion = "It's a beautiful day in Vancouver! Wear sunglasses.";

        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .ReturnsAsync(coordinates);
        _weatherServiceMock
            .Setup(x => x.GetCurrentWeatherAsync(49.25, -123.12))
            .ReturnsAsync(CreateWeatherData());
        _conciergeAgentServiceMock
            .Setup(x => x.GetConciergeResponseAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedSuggestion);

        var result = await _sut.ExecuteAsync("Vancouver");

        Assert.Equal(expectedSuggestion, result);
    }

    [Fact]
    public async Task ExecuteAsync_CallsServicesInCorrectOrder()
    {
        var callOrder = new List<string>();
        var coordinates = (Latitude: 49.25, Longitude: -123.12);

        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .Callback(() => callOrder.Add("geolocation"))
            .ReturnsAsync(coordinates);
        _weatherServiceMock
            .Setup(x => x.GetCurrentWeatherAsync(It.IsAny<double>(), It.IsAny<double>()))
            .Callback(() => callOrder.Add("weather"))
            .ReturnsAsync(CreateWeatherData());
        _conciergeAgentServiceMock
            .Setup(x => x.GetConciergeResponseAsync(It.IsAny<string>()))
            .Callback(() => callOrder.Add("concierge"))
            .ReturnsAsync("suggestion");

        await _sut.ExecuteAsync("Vancouver");

        Assert.Equal(["geolocation", "weather", "concierge"], callOrder);
    }

    [Fact]
    public async Task ExecuteAsync_WhenGeolocationThrows_PropagatesException()
    {
        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .ThrowsAsync(new HttpRequestException("Network error"));

        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.ExecuteAsync("Vancouver"));
    }

    [Fact]
    public async Task ExecuteAsync_WhenWeatherServiceThrows_PropagatesException()
    {
        var coordinates = (Latitude: 49.25, Longitude: -123.12);

        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .ReturnsAsync(coordinates);
        _weatherServiceMock
            .Setup(x => x.GetCurrentWeatherAsync(It.IsAny<double>(), It.IsAny<double>()))
            .ThrowsAsync(new HttpRequestException("Weather API unavailable"));

        await Assert.ThrowsAsync<HttpRequestException>(() => _sut.ExecuteAsync("Vancouver"));
    }

    [Fact]
    public async Task ExecuteAsync_WhenConciergeThrows_PropagatesException()
    {
        var coordinates = (Latitude: 49.25, Longitude: -123.12);

        _geolocationServiceMock
            .Setup(x => x.GetCoordinatesAsync("Vancouver"))
            .ReturnsAsync(coordinates);
        _weatherServiceMock
            .Setup(x => x.GetCurrentWeatherAsync(It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync(CreateWeatherData());
        _conciergeAgentServiceMock
            .Setup(x => x.GetConciergeResponseAsync(It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("AI service error"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ExecuteAsync("Vancouver"));
    }

    private static Weather CreateWeatherData(float temperature = 10f, float apparentTemp = 8f, int precipitationProb = 5)
    {
        return new Weather
        {
            Latitude = 49.25f,
            Longitude = -123.12f,
            Current = new Current
            {
                Temperature2m = temperature,
                ApparentTemperature = apparentTemp,
                PrecipitationProbability = precipitationProb,
                Time = "2026-02-16T12:00",
                Interval = 900
            }
        };
    }
}
