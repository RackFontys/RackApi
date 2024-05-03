FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RackApi.IntegrationTest.csproj", "RackApi.IntegrationTest/"]
RUN dotnet restore "RackApi.IntegrationTest/RackApi.IntegrationTest.csproj"
COPY . .
WORKDIR "/src/RackApi.IntegrationTest"
RUN dotnet build "RackApi.IntegrationTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RackApi.IntegrationTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

ENTRYPOINT ["dotnet", "test", "--logger:trx"]