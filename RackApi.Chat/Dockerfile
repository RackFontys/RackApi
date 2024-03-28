FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RackApi.Chat/RackApi.Chat.csproj", "RackApi.Chat/"]
RUN dotnet restore "RackApi.Chat/RackApi.Chat.csproj"
COPY . .
WORKDIR "/src/RackApi.Chat"
RUN dotnet build "RackApi.Chat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RackApi.Chat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5012
ENTRYPOINT ["dotnet", "RackApi.Chat.dll"]
