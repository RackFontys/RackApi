#!/bin/bash

until curl --output /dev/null --silent --head --fail https://localhost:15672/api/healthchecks/node; do
  echo "Waiting for RabbitMQ to start..."
  sleep 1
done
echo "RabbitMQ is ready!"