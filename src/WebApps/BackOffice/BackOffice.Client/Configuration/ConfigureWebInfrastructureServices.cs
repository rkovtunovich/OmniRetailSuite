using BackOffice.Application.Services.Implementation;
using BackOffice.Application.Services.Implementation.ProductCatalog;
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
        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));
        services.Configure<IdentityClientSettings>(configuration.GetSection($"HttpClients:IdentityClient"));
        services.Configure<ProductCatalogClientSettings>(configuration.GetSection($"HttpClients:ProductCatalogClient"));
        services.Configure<RetailClientSettings>(configuration.GetSection($"HttpClients:RetailClient"));

        services.AddSignalR(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
        services.AddHttpClient(ClientNames.IDENTITY, client =>
        {
            client.BaseAddress = new Uri(configuration[ClientNames.WEB_GATEWAY] ?? throw new Exception("Api Gateway isn't settled in configuration"));
        });
        services.AddHttpClient(ClientNames.PRODUCT_CATALOG, client =>
        {
            client.BaseAddress = new Uri(configuration[ClientNames.WEB_GATEWAY] ?? throw new Exception("Api Gateway isn't settled in configuration"));
        });
        services.AddHttpClient(ClientNames.RETAIL, client =>
        {
            client.BaseAddress = new Uri(configuration[ClientNames.WEB_GATEWAY] ?? throw new Exception("Api Gateway isn't settled in configuration"));
        });

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddKeyedScoped<IHttpService<IdentityClientSettings>, IdentityHttpService>(ClientNames.IDENTITY);
        services.AddKeyedScoped<IHttpService<ProductCatalogClientSettings>, ProductCatalogHttpService>(ClientNames.PRODUCT_CATALOG);
        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));
        
        services.AddSingleton<IdentityUriResolver>();
        services.AddSingleton<ProductCatalogUriResolver>();
        services.AddSingleton<RetailUrlResolver>();

        return services;
    }
}
