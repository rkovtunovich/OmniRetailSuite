using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAppsGateway.Services.Implementation;

public class ProductCatalogServiceHealthCheck(IServiceHealthChecker serviceHealthChecker) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var productCatalogServiceUrl = context.Registration.Name;
        var isHealthy = await serviceHealthChecker.CheckServiceHealth(productCatalogServiceUrl);

        return isHealthy ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
    }
}
