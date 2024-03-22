FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the shared projects
COPY ../../Shared .

# Copy the csproj and restore as distinct layers
WORKDIR /app/src/Identity/Identity.Api
COPY . ./
RUN dotnet restore
RUN mkdir -p /app/published
RUN dotnet publish -c Release -o /app/published || exit 1

# Use the .NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build-env /app/published .
#ENTRYPOINT ["./docker-entrypoint.sh"]
ENTRYPOINT ["dotnet", "Identity.Api.dll"]