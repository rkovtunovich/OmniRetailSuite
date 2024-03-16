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
#===============================================================================
#bash postgres/get-credentials.sh
echo "Waiting for Vault to start and become active..."

# Wait for Vault to become unsealed and active
MAX_RETRIES=10
RETRY_INTERVAL=10
for ((i=1; i<=MAX_RETRIES; i++)); do
    VAULT_STATUS=$(curl -s "$VAULT_ADDR/v1/sys/health" | jq -r '. | {sealed, standby, errors}')
    VAULT_SEALED=$(echo "$VAULT_STATUS" | jq -r '.sealed')
    VAULT_STANDBY=$(echo "$VAULT_STATUS" | jq -r '.standby')
    VAULT_ERRORS=$(echo "$VAULT_STATUS" | jq -r '.errors')

    if [[ "$VAULT_SEALED" == "false" ]] && [[ "$VAULT_STANDBY" == "false" ]] && [[ "$VAULT_ERRORS" == "null" ]]; then
        echo "Vault is active and ready."
        break
    fi

    echo "Retry $i/$MAX_RETRIES: Vault is not ready (Sealed: $VAULT_SEALED, Standby: $VAULT_STANDBY, Errors: $VAULT_ERRORS)"
    sleep $RETRY_INTERVAL
done

# If Vault is not active after the max retries, exit the script
if [[ "$VAULT_SEALED" != "false" ]] || [[ "$VAULT_STANDBY" != "false" ]] || [[ "$VAULT_ERRORS" != "null" ]]; then
    echo "Vault is not active after $MAX_RETRIES retries. Exiting..."
    exit 1
fi

# Authenticate with Vault and retrieve the credentials
echo "Authenticating with Vault and retrieving the credentials..."
POSTGRES_RESPONSE=$(curl --header "X-Vault-Token: $VAULT_TOKEN" --location "$VAULT_ADDR/v1/kv/data/postgres-root")
#echo "POSTGRES_RESPONSE: $POSTGRES_RESPONSE"

# Extract the credentials from the response
POSTGRES_CREDENTIALS=$(echo $POSTGRES_RESPONSE | jq -r .data.data)
#echo "POSTGRES_CREDENTIALS: $POSTGRES_CREDENTIALS"

# Extract the username and password from the credentials
POSTGRES_USER=$(echo "$POSTGRES_CREDENTIALS" | jq -r '.sname')
POSTGRES_PASSWORD=$(echo "$POSTGRES_CREDENTIALS" | jq -r '.spassword')

# Check if we got the credentials
if [[ -z "$POSTGRES_USER" ]] || [[ -z "$POSTGRES_PASSWORD" ]]; then
    echo "Failed to extract credentials."
    exit 1
fi

# Export for use by the PostgreSQL server
export POSTGRES_USER
export POSTGRES_PASSWORD

#--------------------------------------------------------------------------------

echo "POSTGRES_USER: ${POSTGRES_USER}"

echo "Switching to postgres user and starting the PostgreSQL server..."
exec /usr/local/bin/docker-entrypoint.sh postgres
