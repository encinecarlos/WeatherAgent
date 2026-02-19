using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAgent.Application.DTO;
using WeatherAgent.Application.WeatherSuggestion;

namespace WeatherAgent.API;

public class Concierge(
    ILogger<Concierge> logger, IWeatherSuggestionCommand weatherSuggestionCommand)
{
    [Function("Concierge")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "concierge")] HttpRequestData req)
    {
        logger.LogInformation("Concierge endpoint called. Request Method: {Method}, URL: {Url}", req.Method, req.Url);

        try
        {
            var location = req.Query["location"];

            if (string.IsNullOrEmpty(location))
            {
                logger.LogWarning("Location parameter is missing or empty");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new AgentResponseDto("Location parameter is required"));
                return badResponse;
            }

            logger.LogInformation("Processing request for location: {Location}", location);

            var result = await weatherSuggestionCommand.ExecuteAsync(location);

            logger.LogInformation("Successfully generated weather suggestion for location: {Location}", location);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new AgentResponseDto(result));

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Concierge request");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new AgentResponseDto("An error occurred processing your request"));
            return errorResponse;
        }
    }
}