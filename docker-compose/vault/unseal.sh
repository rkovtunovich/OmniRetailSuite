#!/bin/bash

status=$?

# add delay to wait for Vault to start
sleep 10

# Check if Vault is sealed
cmd="vault status"
sealed=$(eval $cmd | grep Sealed | awk '{print $2}')

echo "Vault sealed status: $sealed"

if [ "$sealed" == "true" ]; then
    echo "Vault is sealed. Unsealing..."

    # Provide the unseal keys to Vault
    vault operator unseal $VAULT_UNSEAL_KEY1
    vault operator unseal $VAULT_UNSEAL_KEY2
    vault operator unseal $VAULT_UNSEAL_KEY3
else
    echo "Vault is already unsealed."
fi