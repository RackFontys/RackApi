#!/bin/bash

cat <<EOF > ./appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Ocelot": {
    "UserService": "$OCELLOT_USER_SERVICE",
    "MessageService": "$OCELLOT_MESSAGE_SERVICE",
    "DefaultAddress": "$OCELLOT_DEFAULT_ADDRESS"
  }
}
EOF