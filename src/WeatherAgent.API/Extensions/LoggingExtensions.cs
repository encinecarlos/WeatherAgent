using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace WeatherAgent.API.Extensions
{
    public static class LoggingExtensions
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfigurationManager configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(
                configuration["ApplicationInsights:InstrumentationKey"],
                TelemetryConverter.Traces,
                restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            return services;
        }
    }
}
