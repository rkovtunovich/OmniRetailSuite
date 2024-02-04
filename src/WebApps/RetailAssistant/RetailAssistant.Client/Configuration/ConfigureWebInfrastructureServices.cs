using RetailAssistant.Application.Services.Implementation;
using RetailAssistant.Core.Models.ExternalResources;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddRetailAssistantWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));

        var gatewayUrl = configuration[Constants.HTTP_WEB_GATEWAY] ?? throw new Exception("Gateway isn't settled in configuration");

        services.AddHttpClient(IdentityResource.DefaultClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        services.AddHttpClient(RetailResource.DefaultClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });

        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

        return services;
    }
}
