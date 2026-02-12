using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAgent.Infrastructure.Geolocation;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.API;

public class Concierge(
    ILogger<Concierge> logger,
    IGeolocationService geolocationService,
    IWeatherService weatherService)
{
    [Function("Concierge")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "concierge")] HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var location = req.Query["location"];

        var result = await geolocationService.GetCoordinatesAsync(location);

        var weather = await weatherService.GetCurrentWeatherAsync(result.Value.Latitude, result.Value.Longitude);

        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(new
        {
            Temperature = $"{weather.Current.Temperature2m} {weather.CurrentUnits.Temperature2m}",
            ApparentTemperature = $"{weather.Current.ApparentTemperature} {weather.CurrentUnits.ApparentTemperature}",
            PrecipitationProbability = $"{weather.Current.PrecipitationProbability} {weather.CurrentUnits.PrecipitationProbability}"
        });

        return response;
    }
}