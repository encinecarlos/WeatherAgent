# WeatherAgent

An AI-powered weather concierge application that provides personalized weather insights and suggestions using Azure OpenAI. The application combines real-time weather data with intelligent AI analysis to deliver contextual recommendations based on current weather conditions.

## 🌟 Overview

WeatherAgent is a full-stack application built with a clean architecture approach, featuring:
- **AI-Powered Insights**: Leverages Azure OpenAI (GPT-4o-mini) to provide intelligent weather suggestions
- **Real-Time Weather Data**: Integrates with Open-Meteo API for accurate weather information
- **Modern Tech Stack**: Built with .NET 10 and React 19
- **Serverless Architecture**: Deployed on Azure Functions for scalability and cost-efficiency
- **Observability**: Integrated Application Insights for monitoring and telemetry

## 🏗️ Architecture

The project follows Clean Architecture principles with clear separation of concerns:

```
WeatherAgent/
├── src/
│   ├── WeatherAgent.API/          # Azure Functions HTTP endpoints
│   ├── WeatherAgent.Application/  # Business logic and use cases
│   ├── WeatherAgent.Domain/       # Domain entities and configurations
│   ├── WeatherAgent.Infrastructure/ # External services integration
│   └── weatheragent-web/          # React frontend application
```

### Architecture Layers

- **API Layer**: Azure Functions serving as HTTP endpoints
- **Application Layer**: Orchestrates business logic and use cases
- **Domain Layer**: Core business entities and configuration models
- **Infrastructure Layer**: External service integrations (Azure OpenAI, Weather API, Geocoding)
- **Web Layer**: React-based user interface

## 🚀 Technologies

### Backend
- **.NET 10.0** - Latest .NET framework
- **Azure Functions v4** - Serverless compute platform
- **Azure OpenAI** - AI agent for weather suggestions (GPT-4o-mini)
- **Microsoft.Agents.AI** - AI agent orchestration framework
- **Application Insights** - Application performance monitoring
- **Serilog** - Structured logging with enrichers
- **OpenTelemetry** - Distributed tracing and metrics
- **RestSharp** - HTTP client for external API calls

### Frontend
- **React 19** - Modern UI library
- **Vite 7** - Fast build tool and dev server
- **React Router DOM 7** - Client-side routing
- **Express** - Production server
- **Node.js 24+** - JavaScript runtime

## ☁️ Azure Services

The application leverages the following Azure services:

1. **Azure Functions**
   - Serverless HTTP endpoints
   - Isolated worker runtime (.NET)
   - Auto-scaling capability

2. **Azure OpenAI**
   - GPT-4o-mini model deployment
   - AI agent for weather suggestions
   - Managed AI inference

3. **Application Insights**
   - Real-time monitoring
   - Telemetry and diagnostics
   - Performance tracking
   - Log aggregation

4. **Azure Identity**
   - Managed identity support
   - DefaultAzureCredential for authentication
   - Secure credential management

## 🔧 Prerequisites

Before running the application locally, ensure you have:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 24+](https://nodejs.org/)
- [Azure Functions Core Tools v4](https://docs.microsoft.com/azure/azure-functions/functions-run-local)
- [Azure OpenAI resource](https://azure.microsoft.com/products/ai-services/openai-service) with GPT-4o-mini deployment
- Azure subscription (for Application Insights)

## 🎯 Running Locally

### Backend (Azure Functions API)

1. **Navigate to the API project:**
   ```bash
   cd src/WeatherAgent.API
   ```

2. **Configure local settings:**
   
   Update `local.settings.json` with your Azure OpenAI credentials:
   ```json
   {
     "IsEncrypted": false,
     "Values": {
       "AzureWebJobsStorage": "UseDevelopmentStorage=true",
       "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
       "WeatherConfiguration:BaseUrl": "https://api.open-meteo.com/v1",
       "WeatherConfiguration:GeocodingUrl": "https://geocoding-api.open-meteo.com/v1",
       "AIConfiguration:BaseUrl": "https://your-openai-resource.openai.azure.com/",
       "AIConfiguration:ApiKey": "your-api-key-or-use-managed-identity"
     },
     "Host": {
       "CORS": "*",
       "CORSCredentials": false
     }
   }
   ```

   > **Note**: You can use `DefaultAzureCredential` by omitting the `ApiKey` if you're authenticated with Azure CLI.

3. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

4. **Run the Azure Functions:**
   ```bash
   func start
   ```

   The API will be available at `http://localhost:7071`

### Frontend (React Application)

1. **Navigate to the web project:**
   ```bash
   cd src/weatheragent-web
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Configure environment variables:**
   
   Create or update `.env.development`:
   ```env
   VITE_API_BASE_URL=http://localhost:7071
   ```

4. **Start the development server:**
   ```bash
   npm run dev
   ```

   The application will be available at `http://localhost:5173`

## 🧪 API Endpoints

### GET /api/concierge

Returns AI-powered weather suggestions for a given location.

**Query Parameters:**
- `location` (required) - City name or location to get weather suggestions for

**Example Request:**
```bash
curl "http://localhost:7071/api/concierge?location=London"
```

**Example Response:**
```json
{
  "response": "Currently in London, it's 15°C with clear skies. Great weather for a walk in the park! Consider bringing sunglasses and staying hydrated."
}
```

## 📦 Project Dependencies

### Key NuGet Packages
- `Azure.AI.OpenAI` - Azure OpenAI client
- `Azure.Identity` - Azure authentication
- `Microsoft.Agents.AI` - AI agent framework
- `Microsoft.Azure.Functions.Worker` - Functions worker SDK
- `Microsoft.ApplicationInsights.WorkerService` - Telemetry
- `Serilog.AspNetCore` - Logging framework

### Key NPM Packages
- `react` & `react-dom` - UI framework
- `react-router-dom` - Routing
- `vite` - Build tool
- `express` - Production server

## 🔒 Security Notes

- Never commit `local.settings.json` with real credentials
- Use Azure Managed Identity in production
- Enable HTTPS for production deployments
- Implement proper authentication/authorization for production APIs

## 📝 License

This project is licensed under the MIT License - see [LICENSE.txt](LICENSE.txt) for details.

## ℹ️ Demo Project

This is a sample project created for demonstration purposes to showcase the integration of AI technologies with weather data services. It serves as an educational example of building AI-powered applications using Azure OpenAI and modern web technologies.

---

Built with ❤️ using Azure OpenAI and .NET