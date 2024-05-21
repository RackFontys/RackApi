# Use the official .NET SDK image for build and publish stages
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["RackApi.IntegrationTest/RackApi.IntegrationTest.csproj", "RackApi.IntegrationTest/"]
RUN dotnet restore "RackApi.IntegrationTest/RackApi.IntegrationTest.csproj"

# Copy the remaining source code and build the project
COPY . .
WORKDIR "/src/RackApi.IntegrationTest"
RUN dotnet build "RackApi.IntegrationTest.csproj" -c Release -o /app/build

# Use the same SDK image for the final stage to ensure dotnet CLI is available
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS final
WORKDIR /app
COPY --from=build /app/build .

# Keep the container running
ENTRYPOINT ["tail", "-f", "/dev/null"]