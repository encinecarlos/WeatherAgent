

using System.Globalization;
using RestSharp;
using WeatherAgent.Domain.Configuration;

namespace WeatherAgent.Infrastructure.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IRestClient _restClient;
        private readonly WeatherConfiguration _weatherConfiguration;
        public WeatherService(WeatherConfiguration weatherConfiguration)
        {
            _weatherConfiguration = weatherConfiguration;
            _restClient = new RestClient(_weatherConfiguration.BaseUrl);
        }

        public async Task<Domain.Entities.Weather> GetCurrentWeatherAsync(double latitude, double longitude)
        {
            var request = new RestRequest("/forecast", Method.Get);
            request
                .AddQueryParameter("latitude", latitude.ToString(CultureInfo.InvariantCulture))
                .AddQueryParameter("longitude", longitude.ToString(CultureInfo.InvariantCulture))
                .AddQueryParameter("current", "temperature_2m,apparent_temperature,precipitation_probability")
                .AddQueryParameter("timezone", "auto");

            var response = await _restClient.ExecuteAsync<Domain.Entities.Weather>(request);

            return response.Data;
        }
    }
}
