﻿#!/bin/bash

cat > ./appsettings.json <<EOF
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "RABBITMQ_HOST": "rabbitmq",
    "RABBITMQ_PORT": "5672",
    "RABBITMQ_USERNAME": "$RABBITMQ_USERNAME",
    "RABBITMQ_PASSWORD": "$RABBITMQ_PASSWORD"
  },
  "JsonWebTokenStrings": {
    "IssuerIp": "$ISSUER_IP",
    "AudienceIp": "$AUDIENCE_IP",
    "DefaultJWTKey": "$JWT_KEY"
  },
  "DataBaseStrings": {
    "ApplyMigrations": "false",
    "POSTGRES_HOST": "postgres-users",
    "POSTGRES_PORT": "5432",
    "POSTGRES_DB": "RackUsers",
    "POSTGRES_USERNAME": "$POSTGRES_USERNAME",
    "POSTGRES_PASSWORD": "$POSTGRES_PASSWORD"
  },
}
EOF
