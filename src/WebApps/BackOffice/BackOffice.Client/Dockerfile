FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy all solution files
COPY . ./

# Restore the project
RUN dotnet restore src/WebApps/BackOffice/BackOffice.Client/BackOffice.Client.csproj

# Publish the project
RUN dotnet publish src/WebApps/BackOffice/BackOffice.Client/BackOffice.Client.csproj -c Release -o published 

## Use the .NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
COPY --from=build-env app/published .

ENTRYPOINT ["dotnet", "BackOffice.Client.dll"]