#!/bin/bash

json_content=$(cat <<EOF
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
    "POSTGRES_HOST": "postgres-chats",
    "POSTGRES_PORT": "5432",
    "POSTGRES_DB": "RackChats",
    "POSTGRES_USERNAME": "$POSTGRES_USERNAME",
    "POSTGRES_PASSWORD": "$POSTGRES_PASSWORD"
  },
}
EOF
)

json_file="appsettings.json"

echo "$json_content" > "$json_file"

echo "JSON file '$json_file' created."