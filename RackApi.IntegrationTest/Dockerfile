FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . ./
WORKDIR /app/IntegrationTests
RUN dotnet restore
RUN dotnet build -c Release --no-restore

ENTRYPOINT ["dotnet", "test", "--logger:trx"]