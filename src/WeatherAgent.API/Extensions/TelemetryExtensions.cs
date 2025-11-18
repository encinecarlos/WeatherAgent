using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WeatherAgent.API.Extensions
{
    public static class TelemetryExtensions
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfigurationManager configuration)
        {
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName: "WeatherAgentAPI", serviceVersion: "1.0.0");

            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder.SetResourceBuilder(resourceBuilder)
                    .AddSource("WeaterAgentAPI")
                    .AddHttpClientInstrumentation()
                    .AddAzureMonitorTraceExporter(options =>
                    {
                        options.ConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
                    });
                }).WithMetrics(builder =>
                {
                    builder.SetResourceBuilder(resourceBuilder)
                        .AddHttpClientInstrumentation()
                        .AddAzureMonitorMetricExporter(options =>
                        {
                            options.ConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
                        });
                });

            return services;
        }
    }
}
