using WebAppsGateway.Services.Implementation;

namespace WebAppsGateway.Configuration;

public static class ConfigureHealthChecks
{
    public static void AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddSingleton<IServiceHealthChecker, ServiceHealthChecker>();

        services.AddHealthChecks()
            .AddCheck<RetailServiceHealthCheck>("retail")
            .AddCheck<ProductCatalogServiceHealthCheck>("product-catalog");
    }
}
