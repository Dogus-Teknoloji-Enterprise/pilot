# Pilot API Project

A .NET 8.0 Web API project demonstrating weather forecast functionality.

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or Visual Studio Code
- SQL Server (for database operations)

## Getting Started

1. Clone the repository
```bash
git clone <repository-url>
```

2. Navigate to the API project directory
```bash
cd pilot/api
```

3. Restore dependencies
```bash
dotnet restore
```

4. Run the application
```bash
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5166
- HTTPS: https://localhost:7084

## API Endpoints

- GET `/weatherforecast` - Returns weather forecast data
- POST `/weatherforecast` - Creates a new weather forecast entry
- GET `/currency-exchange` - Returns currency exchange rates

## Development

The project uses:
- ASP.NET Core 8.0
- Swagger/OpenAPI for API documentation
- Microsoft.Data.SqlClient for database operations

## Security Note

This project contains example security vulnerabilities for demonstration purposes. Do not use these patterns in production code.
