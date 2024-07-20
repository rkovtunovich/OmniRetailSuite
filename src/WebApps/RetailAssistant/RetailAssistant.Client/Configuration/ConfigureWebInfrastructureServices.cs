using Infrastructure.Http;
using Infrastructure.Http.Clients;
using Infrastructure.Http.Uri;
using RetailAssistant.Core.Models.Settings;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddRetailAssistantWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection(IdentityServerSettings.Key));
        services.Configure<GatewaySettings>(configuration.GetSection(GatewaySettings.Key));

        var identityClientSettings = configuration.GetSection(IdentityClientSettings.Key);
        services.Configure<IdentityClientSettings>(identityClientSettings);
        var identityClientName = identityClientSettings.GetValue<string>(nameof(IdentityClientSettings.Name)) 
            ?? throw new Exception("Identity client name isn't settled in configuration");

        var authority = configuration.GetValue<string>("IdentityServerSettings:Authority") ??
                throw new Exception("Identity server authority isn't settled in configuration");

        services.AddHttpClient(identityClientName, client =>
        {
            client.BaseAddress = new Uri(authority); 
        });

        var gatewaySettings = configuration.GetSection(GatewaySettings.Key);
        var gatewayUrl = gatewaySettings.GetValue<string>(nameof(GatewaySettings.BaseUrl)) 
            ?? throw new Exception("Gateway BaseUrl isn't settled in configuration");

        var productCatalogClientSettings = configuration.GetSection(ProductCatalogClientSettings.Key);
        services.Configure<ProductCatalogClientSettings>(productCatalogClientSettings);
        var productCatalogClientName = productCatalogClientSettings.GetValue<string>(nameof(ProductCatalogClientSettings.Name)) 
            ?? throw new Exception("Product catalog client name isn't settled in configuration");
        services.AddHttpClient(productCatalogClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        var retailClientSettings = configuration.GetSection(RetailClientSettings.Key);
        services.Configure<RetailClientSettings>(retailClientSettings);
        var retailClientName = retailClientSettings.GetValue<string>(nameof(RetailClientSettings.Name)) 
            ?? throw new Exception("Retail client name isn't settled in configuration");

        services.AddHttpClient(retailClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        services.AddSingleton<IApplicationStateService, ApplicationStateService>();
        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

        services.AddSingleton<IdentityUriResolver>();
        services.AddSingleton<ProductCatalogUriResolver>();
        services.AddSingleton<RetailUrlResolver>();

        return services;
    }
}
