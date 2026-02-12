using Microsoft.Extensions.DependencyInjection;
using WeatherAgent.Application.WeatherSuggestion;

namespace WeatherAgent.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherSuggestionCommand, WeatherSuggestionCommand>();

            return services;
        }
    }
}
