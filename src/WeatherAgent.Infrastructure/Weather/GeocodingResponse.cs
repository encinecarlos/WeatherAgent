namespace WeatherAgent.Infrastructure.Weather
{
    public class GeocodingResponse
    {
        public List<GeocodingResult>? Results { get; set; }
    }

    public class GeocodingResult
    {
        public string? Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Country { get; set; }
        public string? Admin1 { get; set; }
    }
}
