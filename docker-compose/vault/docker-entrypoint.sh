#!/bin/bash

# Start Vault in the background
echo "Starting Vault..."
/vault/vault server -config=/vault/config/vault-config.json &
VAULT_PID=$!

# Wait for Vault to start up
echo "Waiting for Vault to start..."
sleep 10

# Check if Vault is up by hitting the sys/health endpoint.
# Adjust the number of retries as necessary.
MAX_RETRIES=5
RETRY_INTERVAL=5
for ((i=1;i<=MAX_RETRIES;i++)); do
    curl -s http://127.0.0.1:8200/v1/sys/health && break
    echo "Retry $i/$MAX_RETRIES failed, waiting $RETRY_INTERVAL seconds..."
    sleep $RETRY_INTERVAL
done

# Run the unseal script
echo "Attempting to unseal Vault..."
sh /vault/unseal.sh

# Keep the script running to keep the container alive.
# This waits for the Vault process to exit.
wait $VAULT_PID
