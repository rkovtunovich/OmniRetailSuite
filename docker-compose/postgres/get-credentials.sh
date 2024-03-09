#!/bin/bash

# Read the VAULT_ADDR and VAULT_TOKEN from the environment variables
VAULT_ADDR=${VAULT_ADDR}
VAULT_TOKEN=${VAULT_TOKEN}

echo "VAULT_ADDR: $VAULT_ADDR"
echo "VAULT_TOKEN: $VAULT_TOKEN"

# Authenticate with Vault and retrieve the credentials
POSTGRES_CREDENTIALS=$(curl --header "X-Vault-Token: $VAULT_TOKEN" $VAULT_ADDR/v1/kv/data/postgres | jq .data.data)

# Extract the username and password from the credentials
POSTGRES_USER=$(echo $POSTGRES_CREDENTIALS | jq -r .superuser-name)
POSTGRES_PASSWORD=$(echo $POSTGRES_CREDENTIALS | jq -r .superuser-password)

# Export for use by the PostgreSQL server
export POSTGRES_USER
export POSTGRES_PASSWORD

# Call the original entrypoint script
exec docker-entrypoint.sh postgres
