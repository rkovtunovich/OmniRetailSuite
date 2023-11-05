#!/usr/bin/env bash

# exit when any command fails
set -e

# trust dev root CA
#openssl x509 -inform DER -in /https/root/eshop-root-cert.cer -out /https/root/eshop-root-cert.crt
echo "copying the root certificate"
cp /https/root/eshop-root-cert.crt /usr/local/share/ca-certificates/
echo "updating the certificate store"
update-ca-certificates

# start the app
dotnet watch run