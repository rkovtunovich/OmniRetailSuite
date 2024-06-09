using BackOffice.Application.Services.Implementation;
using BackOffice.Client.Exceptions;
using BackOffice.Core.Models.Settings;
using Infrastructure.Http;
using Infrastructure.Http.Clients;
using Infrastructure.Http.Uri;
using Microsoft.AspNetCore.HttpLogging;

namespace BackOffice.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger<Program>();

        var identityServerSettings = configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>();

        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));
        services.Configure<IdentityClientSettings>(configuration.GetSection($"HttpClients:IdentityClient"));
        services.Configure<ProductCatalogClientSettings>(configuration.GetSection($"HttpClients:ProductCatalogClient"));
        services.Configure<RetailClientSettings>(configuration.GetSection($"HttpClients:RetailClient"));

        services.AddSignalR(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);

        logger.LogInformation("Configuring Http Clients...");
        logger.LogInformation("IdentityServer: {IdentityServer}", identityServerSettings?.Authority);
        logger.LogInformation("ProductCatalog: {ProductCatalog}", configuration[ClientNames.WEB_GATEWAY]);
        logger.LogInformation("Retail: {Retail}", configuration[ClientNames.WEB_GATEWAY]);

        services.AddHttpClient(ClientNames.IDENTITY, client =>
        {
            client.BaseAddress = new Uri(identityServerSettings?.Authority 
                ?? throw new AbsentConfigurationParameterException($"Identity Server isn't settled in configuration {nameof(IdentityServerSettings.Authority)}"));
        });

        services.AddHttpClient(ClientNames.PRODUCT_CATALOG, client =>
        {
            client.BaseAddress = new Uri(configuration[ClientNames.WEB_GATEWAY] 
                ?? throw new AbsentConfigurationParameterException("Api Gateway isn't settled in configuration"));
        });

        services.AddHttpClient(ClientNames.RETAIL, client =>
        {
            client.BaseAddress = new Uri(configuration[ClientNames.WEB_GATEWAY] 
                ?? throw new AbsentConfigurationParameterException("Api Gateway isn't settled in configuration"));
        });

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddKeyedScoped<IHttpService<IdentityClientSettings>, IdentityHttpService>(ClientNames.IDENTITY);
        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));
        
        services.AddSingleton<IdentityUriResolver>();
        services.AddSingleton<ProductCatalogUriResolver>();
        services.AddSingleton<RetailUrlResolver>();

        return services;
    }
}
