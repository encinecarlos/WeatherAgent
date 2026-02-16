using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAgent.Application.DTO;
using WeatherAgent.Application.WeatherSuggestion;

namespace WeatherAgent.API;

public class Concierge(
    ILogger<Concierge> logger, IWeatherSuggestionCommand weatherSugestionCommand)
{
    [Function("Concierge")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "concierge")] HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var location = req.Query["location"];

        var result = await weatherSugestionCommand.ExecuteAsync(location);

        logger.LogInformation("Concierge response: {response}", result);

        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(new AgentResponseDto(result));

        return response;
    }
}