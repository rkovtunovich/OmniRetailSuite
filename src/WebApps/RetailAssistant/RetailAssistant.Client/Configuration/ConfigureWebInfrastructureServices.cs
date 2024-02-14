using Infrastructure.Http;
using Infrastructure.Http.Clients;
using RetailAssistant.Application.Services.Implementation;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddRetailAssistantWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection(IdentityServerSettings.Key));

        var gatewayUrl = configuration[Constants.HTTP_WEB_GATEWAY] ?? throw new Exception("Gateway isn't settled in configuration");

        var identityClientSettings = configuration.GetSection(IdentityClientSettings.Key);
        services.Configure<IdentityClientSettings>(identityClientSettings);
        var identityClientName = identityClientSettings.GetValue<string>(nameof(IdentityClientSettings.Name)) ?? throw new Exception("Identity client name isn't settled in configuration");
        services.AddHttpClient(identityClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        var productCatalogClientSettings = configuration.GetSection(ProductCatalogClientSettings.Key);
        services.Configure<ProductCatalogClientSettings>(productCatalogClientSettings);
        var productCatalogClientName = productCatalogClientSettings.GetValue<string>(nameof(ProductCatalogClientSettings.Name)) ?? throw new Exception("Product catalog client name isn't settled in configuration");
        services.AddHttpClient(productCatalogClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        var retailClientSettings = configuration.GetSection(RetailClientSettings.Key);
        services.Configure<RetailClientSettings>(retailClientSettings);
        var retailClientName = retailClientSettings.GetValue<string>(nameof(RetailClientSettings.Name)) ?? throw new Exception("Retail client name isn't settled in configuration");

        services.AddHttpClient(retailClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

        return services;
    }
}
