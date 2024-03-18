# Use the official .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/out ./

# Expose port 80 to the outside world
EXPOSE 5283

# Define the command to run the application
ENTRYPOINT ["dotnet", "RackApi.dll"]