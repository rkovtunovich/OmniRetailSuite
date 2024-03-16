#!/bin/bash

# Vault address
VAULT_ADDR=${VAULT_ADDR}

echo "Vault address: $VAULT_ADDR"
 
# Wait for Vault to start up
echo "Waiting for Vault to start..."
sleep 10

# Check if Vault is up and unsealed by hitting the sys/health endpoint.
# Adjust the number of retries as necessary.
MAX_RETRIES=10
RETRY_INTERVAL=10

for ((i=1; i<=MAX_RETRIES; i++)); do
    VAULT_HEALTH=$(curl -s "$VAULT_ADDR/v1/sys/health")

    # Check if the response includes '"sealed":false' to indicate that Vault is unsealed
    if echo "$VAULT_HEALTH" | grep -q '"sealed":false'; then
        echo "Vault is unsealed."
        break
    fi

    echo "Retry $i/$MAX_RETRIES failed, Vault is sealed, waiting $RETRY_INTERVAL seconds..."
    sleep $RETRY_INTERVAL
done

# Add additional logic to handle the case where Vault remains sealed after all retries
if echo "$VAULT_HEALTH" | grep -q '"sealed":true'; then
    echo "Vault is still sealed after $MAX_RETRIES retries. Exiting..."
    exit 1
fi

# run get credentials script
echo "Running get credentials script..."
bash postgres/get-credentials.sh

echo "Switching to postgres user and starting the PostgreSQL server..."
exec /usr/local/bin/docker-entrypoint.sh postgres
