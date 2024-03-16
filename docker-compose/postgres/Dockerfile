FROM postgres:latest

# Install curl and jq
RUN apt-get update && apt-get install -y curl jq && rm -rf /var/lib/apt/lists/*

# Copy the init script into the container
COPY get-credentials.sh /postgres/get-credentials.sh
# make the entrypoint script executable
RUN chmod +x /postgres/get-credentials.sh

# add the entrypoint script
COPY docker-entrypoint.sh /postgres/docker-entrypoint.sh
# make the entrypoint script executable
RUN chmod +x /postgres/docker-entrypoint.sh

# Copy config file
#COPY postgresql.conf /var/lib/postgresql/data/postgresql.conf

# run postgres
ENTRYPOINT ["postgres/docker-entrypoint.sh"]