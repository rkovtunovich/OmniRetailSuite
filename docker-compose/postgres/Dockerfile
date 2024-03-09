FROM postgres:latest

# Copy the init script into the container
COPY get-credentials.sh /docker-entrypoint-initdb.d/

# Make it executable
RUN chmod +x /docker-entrypoint-initdb.d/get-credentials.sh

# When the container starts, the script will run automatically