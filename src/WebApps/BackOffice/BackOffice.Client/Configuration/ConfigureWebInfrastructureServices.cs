using BackOffice.Application;
using BackOffice.Application.Services.Implementation;
using BackOffice.Application.Services.Implementation.ProductCatalog;
using BackOffice.Core.Models.ExternalResources;
using BackOffice.Core.Models.Settings;
using Microsoft.AspNetCore.HttpLogging;

namespace BackOffice.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));
        services.AddSignalR(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
        services.AddHttpClient(Constants.IDENTITY_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(configuration[Constants.HTTP_WEB_GATEWAY] ?? throw new Exception("Api Gateway isn't settled in configuration"));
        });
        services.AddHttpClient(Constants.PRODUCT_CATALOG_HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(configuration[Constants.HTTP_WEB_GATEWAY] ?? throw new Exception("Api Gateway isn't settled in configuration"));
        });
        services.AddHttpClient(Constants.RETAIL_HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = new Uri(configuration[Constants.HTTP_WEB_GATEWAY] ?? throw new Exception("Api Gateway isn't settled in configuration"));
        });

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddKeyedScoped<IHttpService<IdentityResource>, IdentityHttpService>(Constants.IDENTITY_CLIENT_NAME);
        services.AddKeyedScoped<IHttpService<ProductCatalogResource>, ProductCatalogHttpService>(Constants.PRODUCT_CATALOG_HTTP_CLIENT_NAME);
        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

        return services;
    }
}
