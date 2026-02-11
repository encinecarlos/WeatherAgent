using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAgent.Infrastructure.Geolocation;

namespace WeatherAgent.API;

public class Concierge(ILogger<Concierge> logger, IGeolocationService geolocationService)
{
    [Function("Concierge")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "concierge")] HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var location = req.Query["location"];

        var result = await geolocationService.GetCoordinatesAsync(location);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteAsJsonAsync(new
        {
            result?.Latitude,
            result?.Longitude
        });

        return response;
    }
}