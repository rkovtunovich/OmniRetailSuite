# base image
FROM alpine:latest

# set vault version
ENV VAULT_VERSION 1.15.6

# create a new directory
RUN mkdir /vault

# download dependencies
RUN apk --no-cache add \
      bash \
      ca-certificates \
      wget \
      curl

# download and set up vault
RUN wget --quiet --output-document=/tmp/vault.zip https://releases.hashicorp.com/vault/${VAULT_VERSION}/vault_${VAULT_VERSION}_linux_amd64.zip && \
    unzip /tmp/vault.zip -d /vault && \
    rm -f /tmp/vault.zip && \
    chmod +x /vault

# update PATH
ENV PATH="PATH=$PATH:$PWD/vault"

# add the config file
COPY vault-consul-backend-config.json /vault/config/vault-config.json

# add the unseal script
COPY unseal.sh /vault/unseal.sh
# make the unseal script executable
RUN chmod +x /vault/unseal.sh

# add the entrypoint script
COPY docker-entrypoint.sh /vault/docker-entrypoint.sh
# make the entrypoint script executable
RUN chmod +x /vault/docker-entrypoint.sh

# expose port 8200
EXPOSE 8200

# run vault
ENTRYPOINT ["/vault/docker-entrypoint.sh"]