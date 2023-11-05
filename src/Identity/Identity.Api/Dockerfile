#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /app
COPY . .
#COPY ["src/PublicApi/PublicApi.csproj", "./PublicApi/"]
#RUN dotnet restore "./PublicApi/PublicApi.csproj"
#COPY . .
WORKDIR "/app/src/Identity.Api"
RUN dotnet restore

RUN dotnet build "./Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Identity.Api.dll"]
ENTRYPOINT ["./docker-entrypoint.sh"]