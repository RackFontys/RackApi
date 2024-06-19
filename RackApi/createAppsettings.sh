#!/bin/bash

json_content=$(cat <<EOF
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JsonWebTokenStrings": {
    "IssuerIp": "$ISSUER_IP",
    "AudienceIp": "$AUDIENCE_IP",
    "DefaultJWTKey": "$JWT_KEY"
  },
  "Ocelot": {
    "UserService": "$OCELLOT_USER_SERVICE",
    "MessageService": "$OCELLOT_MESSAGE_SERVICE",
    "DefaultAddress": "$OCELLOT_DEFAULT_ADDRESS"
  }
}
EOF
)

json_file="./RackApi/appsettings.json"

echo "$json_content" > "$json_file"

echo "JSON file '$json_file' created."