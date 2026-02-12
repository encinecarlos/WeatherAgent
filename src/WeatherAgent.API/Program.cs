using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAgent.API.Extensions;
using WeatherAgent.Domain.Configuration;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
var wconfig = builder.Configuration.GetSection("WeatherConfiguration");
builder.Services.Configure<WeatherConfiguration>(builder.Configuration.GetSection("WeatherConfiguration"));
builder.Services.Configure<AIConfiguration>(builder.Configuration.GetSection("AIConfiguration"));

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
