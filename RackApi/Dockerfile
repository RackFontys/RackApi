﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RackApi.csproj", "RackApi.csproj"]
RUN dotnet restore "RackApi.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "RackApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RackApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["appsettings.json", "appsettings.json"]
EXPOSE 5283
ENTRYPOINT ["dotnet", "RackApi.dll"]
