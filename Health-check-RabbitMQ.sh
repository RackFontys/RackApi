until curl --fail --silent --insecure --output /dev/null "https://localhost:15672/api/healthchecks/node"; do
    sleep 5
    echo "Retrying"
done

echo "Endpoint is up!"