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
        logger.LogInformation("Concierge endpoint called. Request Method: {Method}, URL: {Url}", req.Method, req.Url);

        var location = req.Query["location"];

        if (string.IsNullOrEmpty(location))
        {
            logger.LogWarning("Location parameter is missing or empty");
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(new { 
                success = false,
                error = "Location parameter is required" 
            });
            return badResponse;
        }

        logger.LogInformation("Processing request for location: {Location}", location);

        var result = await weatherSugestionCommand.ExecuteAsync(location);

        if (result.IsFailure)
        {
            logger.LogWarning("Failed to generate weather suggestion for location: {Location}. Error: {Error}", location, result.Error);
            var errorResponse = req.CreateResponse(HttpStatusCode.OK);
            await errorResponse.WriteAsJsonAsync(new { 
                success = false,
                error = result.Error 
            });
            return errorResponse;
        }

        logger.LogInformation("Successfully generated weather suggestion for location: {Location}", location);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { 
            success = true,
            agentResponse = result.Value 
        });

        return response;
    }
}