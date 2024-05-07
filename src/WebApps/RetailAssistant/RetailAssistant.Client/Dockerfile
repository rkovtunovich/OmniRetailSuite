FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy all solution files
COPY . ./

# Restore the project
RUN dotnet restore src/WebApps/RetailAssistant/RetailAssistant.Client/RetailAssistant.Client.csproj

# Publish the project
RUN dotnet publish src/WebApps/RetailAssistant/RetailAssistant.Client/RetailAssistant.Client.csproj -c Release -o published 

FROM nginx AS base
WORKDIR /usr/share/nginx/html
COPY --from=build-env app/published .