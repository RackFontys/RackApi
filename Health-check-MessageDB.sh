#!/bin/bash

# Wait for the endpoint to return a 200 response
until pg_isready -U postgres; do
    sleep 5
    echo "Retrying"
done

echo "Endpoint is up!"