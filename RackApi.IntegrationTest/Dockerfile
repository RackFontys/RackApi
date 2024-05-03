FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

# Copy the integration test project file and restore the dependencies
COPY ["IntegrationTests/IntegrationTests.csproj", "RackApi.IntegrationTests/"]
WORKDIR "/app/RackApi.IntegrationTests"
RUN dotnet restore

# Copy the rest of the code and build the project
COPY . .
RUN dotnet build -c Release --no-restore

# Run the tests
ENTRYPOINT ["dotnet", "test", "--logger:trx"]