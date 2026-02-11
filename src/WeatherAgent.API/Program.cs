using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAgent.API.Extensions;
using WeatherAgent.Domain.Configuration;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.Configure<WeatherConfiguration>(builder.Configuration.GetSection("WeatherConfiguration"));

builder.Services
    .ConfigureLogging(builder.Configuration)
    .AddTelemetry(builder.Configuration)
    .AddInfrastructureServices();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
