
FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
#EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /app
COPY . .

WORKDIR "/app/src/BackOffice"
RUN dotnet restore

RUN dotnet build "./BackOffice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./BackOffice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "BackOffice.dll"]
COPY ./docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh
ENTRYPOINT ["./docker-entrypoint.sh"]