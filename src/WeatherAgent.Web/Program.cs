using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherAgent.Web;
using WeatherAgent.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load configuration
var apiBaseUrl = builder.Configuration.GetSection("ApiSettings:BaseUrl").Value 
    ?? "http://localhost:7071";

// Configure HttpClient with API base URL
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(apiBaseUrl) 
});

// Register services
builder.Services.AddScoped<IWeatherAgentService, WeatherAgentService>();

await builder.Build().RunAsync();
