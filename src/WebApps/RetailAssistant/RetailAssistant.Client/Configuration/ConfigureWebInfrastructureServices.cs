using Infrastructure.Http;
using Infrastructure.Http.Clients;
using Infrastructure.Http.Uri;
using Polly;
using Polly.Retry;
using RetailAssistant.Core.Models.Settings;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureWebInfrastructureServices
{
    public static IServiceCollection AddRetailAssistantWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection(IdentityServerSettings.Key));
        services.Configure<GatewaySettings>(configuration.GetSection(GatewaySettings.Key));
      
        services.AddIdentityClient(configuration);
        services.AddProductCataloClient(configuration);
        services.AddRetailClient(configuration);

        services.AddSingleton<IApplicationStateService, ApplicationStateService>();
        services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

        services.AddSingleton<IdentityUriResolver>();
        services.AddSingleton<ProductCatalogUriResolver>();
        services.AddSingleton<RetailUrlResolver>();

        services.AddRetryPolicy(configuration);

        return services;
    }

    private static void AddIdentityClient(this IServiceCollection services, IConfiguration configuration)
    {
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
    }

    private static void AddProductCataloClient(this IServiceCollection services, IConfiguration configuration)
    {
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
    }

    private static void AddRetailClient(this IServiceCollection services, IConfiguration configuration)
    {
        var retailClientSettings = configuration.GetSection(RetailClientSettings.Key);
        services.Configure<RetailClientSettings>(retailClientSettings);
        var retailClientName = retailClientSettings.GetValue<string>(nameof(RetailClientSettings.Name))
            ?? throw new Exception("Retail client name isn't settled in configuration");

        var gatewaySettings = configuration.GetSection(GatewaySettings.Key);
        var gatewayUrl = gatewaySettings.GetValue<string>(nameof(GatewaySettings.BaseUrl))
            ?? throw new Exception("Gateway BaseUrl isn't settled in configuration");

        services.AddHttpClient(retailClientName, client =>
        {
            client.BaseAddress = new Uri(gatewayUrl);
        });
    }

    private static void AddRetryPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var retryPolicySettings = configuration.GetSection(RetryPolicySettings.Key);
        var retryStrategy = new RetryStrategyOptions
        {
            MaxRetryAttempts = retryPolicySettings.GetValue<int>(nameof(RetryPolicySettings.MaxRetryAttempts)),
            Delay = TimeSpan.FromSeconds(retryPolicySettings.GetValue<int>(nameof(RetryPolicySettings.Delay))),
            BackoffType = DelayBackoffType.Exponential
        };

        services.AddResiliencePipeline(RetryPolicySettings.Key, (builder, context) =>
            {
                retryStrategy.OnRetry = arg =>
                {
                    var logger = context.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning($"Retry {retryStrategy.MaxRetryAttempts - arg.AttemptNumber} of {retryStrategy.MaxRetryAttempts} attempts");

                    return default;
                };

                builder.AddRetry(retryStrategy);
            });
    }
}
