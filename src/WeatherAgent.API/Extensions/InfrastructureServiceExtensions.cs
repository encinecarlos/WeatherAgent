using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WeatherAgent.Domain.Configuration;
using WeatherAgent.Infrastructure.Geolocation;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.API.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(sp =>
                sp.GetRequiredService<IOptions<WeatherConfiguration>>().Value);

            services.AddScoped<IGeolocationService, GeolocationService>();
            services.AddScoped<IWeatherService, WeatherService>();

            return services;
        }
    }
}