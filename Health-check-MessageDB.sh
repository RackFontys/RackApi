until pg_isready -U postgres; do
    sleep 5
    echo "Retrying"
done

echo "Endpoint is up!"