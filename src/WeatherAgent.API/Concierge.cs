using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WeatherAgent.Application.WeatherSuggestion;
using WeatherAgent.Domain.Common;

namespace WeatherAgent.API;

public class Concierge(
    ILogger<Concierge> logger,
    IWeatherSuggestionCommand weatherSugestionCommand,
    TelemetryClient telemetryClient)
{
    [Function("Concierge")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "concierge")] HttpRequestData req)
    {
        logger.LogInformation("Concierge endpoint called. Request Method: {Method}, URL: {Url}", req.Method, req.Url);

        var location = req.Query["location"];

        var correlationId = Guid.NewGuid().ToString();

        var metadata = new Dictionary<string, string>
        {
            { "CorrelationId", correlationId },
            { "RequestMethod", req.Method },
            { "RequestUrl", req.Url.ToString() },
            { "Location", location }
        };



        if (string.IsNullOrEmpty(location))
        {
            logger.LogWarning("Location parameter is missing or empty");

            metadata.Add("Error", "Location parameter is required");

            telemetryClient.TrackEvent("ConciergeRequestFailed", metadata);
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteAsJsonAsync(new
            {
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

            metadata.Add("Error", result.Error);

            telemetryClient.TrackEvent("ConciergeRequestFailed", metadata);

            var errorResponse = req.CreateResponse(GetStatusCodeFromError(result.Error));

            await errorResponse.WriteAsJsonAsync(new
            {
                success = false,
                error = result.Error
            });

            return errorResponse;
        }

        logger.LogInformation("Successfully generated weather suggestion for location: {Location}", location);

        telemetryClient.TrackEvent("ConciergeRequestSucceeded", metadata);

        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteAsJsonAsync(new
        {
            success = true,
            agentResponse = result.Value
        });

        return response;
    }

    private static HttpStatusCode GetStatusCodeFromError(string error)
    {
        return error switch
        {
            ErrorMessages.LocationNotFound => HttpStatusCode.NotFound,
            ErrorMessages.WeatherDataNotFound => HttpStatusCode.NotFound,
            ErrorMessages.GeolocationServiceError => HttpStatusCode.ServiceUnavailable,
            ErrorMessages.WeatherServiceError => HttpStatusCode.ServiceUnavailable,
            ErrorMessages.AiAgentError => HttpStatusCode.ServiceUnavailable,
            ErrorMessages.AiAgentResponseEmpty => HttpStatusCode.ServiceUnavailable,
            ErrorMessages.LocationRequired => HttpStatusCode.BadRequest,
            ErrorMessages.LocationInvalid => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}