using WeatherAgent.Domain.Entities;

namespace WeatherAgent.Tests.Domain;

public class WeatherEntityTests
{
    [Fact]
    public void Weather_DefaultValues_AreSetCorrectly()
    {
        var weather = new Weather();

        Assert.Equal(0f, weather.Latitude);
        Assert.Equal(0f, weather.Longitude);
        Assert.Null(weather.Current);
    }

    [Fact]
    public void Weather_SetProperties_ReturnsCorrectValues()
    {
        var weather = new Weather
        {
            Latitude = 49.25f,
            Longitude = -123.12f,
            GenerationTimeMs = 0.5f,
            UtcOffsetSeconds = -28800,
            Timezone = "America/Vancouver",
            TimezoneAbbreviation = "PST",
            Elevation = 70f
        };

        Assert.Equal(49.25f, weather.Latitude);
        Assert.Equal(-123.12f, weather.Longitude);
        Assert.Equal(0.5f, weather.GenerationTimeMs);
        Assert.Equal(-28800, weather.UtcOffsetSeconds);
        Assert.Equal("America/Vancouver", weather.Timezone);
        Assert.Equal("PST", weather.TimezoneAbbreviation);
        Assert.Equal(70f, weather.Elevation);
    }

    [Fact]
    public void Current_SetProperties_ReturnsCorrectValues()
    {
        var current = new Current
        {
            Time = "2026-02-16T12:00",
            Interval = 900,
            Temperature2m = 15.5f,
            ApparentTemperature = 13.2f,
            PrecipitationProbability = 20
        };

        Assert.Equal("2026-02-16T12:00", current.Time);
        Assert.Equal(900, current.Interval);
        Assert.Equal(15.5f, current.Temperature2m);
        Assert.Equal(13.2f, current.ApparentTemperature);
        Assert.Equal(20, current.PrecipitationProbability);
    }

    [Fact]
    public void CurrentUnits_SetProperties_ReturnsCorrectValues()
    {
        var units = new CurrentUnits
        {
            Time = "iso8601",
            Interval = "seconds",
            Temperature2m = "°C",
            ApparentTemperature = "°C",
            PrecipitationProbability = "%"
        };

        Assert.Equal("iso8601", units.Time);
        Assert.Equal("seconds", units.Interval);
        Assert.Equal("°C", units.Temperature2m);
        Assert.Equal("°C", units.ApparentTemperature);
        Assert.Equal("%", units.PrecipitationProbability);
    }

    [Fact]
    public void Weather_WithCurrentData_ReturnsNestedValues()
    {
        var weather = new Weather
        {
            Current = new Current
            {
                Temperature2m = -5.3f,
                ApparentTemperature = -10.1f,
                PrecipitationProbability = 80
            },
            CurrentUnits = new CurrentUnits
            {
                Temperature2m = "°C",
                ApparentTemperature = "°C",
                PrecipitationProbability = "%"
            }
        };

        Assert.Equal(-5.3f, weather.Current.Temperature2m);
        Assert.Equal(-10.1f, weather.Current.ApparentTemperature);
        Assert.Equal(80, weather.Current.PrecipitationProbability);
        Assert.Equal("°C", weather.CurrentUnits.Temperature2m);
    }
}
