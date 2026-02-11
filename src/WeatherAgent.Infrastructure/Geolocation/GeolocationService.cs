using RestSharp;
using WeatherAgent.Domain.Configuration;
using WeatherAgent.Infrastructure.Weather;

namespace WeatherAgent.Infrastructure.Geolocation
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IRestClient _restClient;
        private readonly WeatherConfiguration _weatherConfiguration;

        public GeolocationService(WeatherConfiguration weatherConfiguration)
        {
            _weatherConfiguration = weatherConfiguration;
            _restClient = new RestClient(_weatherConfiguration.Baseurl);
        }

        public async Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string location)
        {
            var request = new RestRequest("/search", Method.Get);
            request
                .AddQueryParameter("name", location)
                .AddQueryParameter("count", 10)
                .AddQueryParameter("language", "pt")
                .AddQueryParameter("format", "json");

            var result = await _restClient.ExecuteAsync<GeocodingResponse>(request);

            var firstResult = result.Data?.Results?.FirstOrDefault();

            return (firstResult.Latitude, firstResult.Longitude);
        }
    }
}
