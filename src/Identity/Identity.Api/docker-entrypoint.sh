#!/usr/bin/env bash

# exit when any command fails
# set -e

# trust dev root CA
#openssl x509 -inform DER -in /https/root/eshop-root-cert.cer -out /https/root/eshop-root-cert.crt
# cp /https/root/eshop-root-cert.crt /usr/local/share/ca-certificates/
# update-ca-certificates

# start the app
dotnet watch run