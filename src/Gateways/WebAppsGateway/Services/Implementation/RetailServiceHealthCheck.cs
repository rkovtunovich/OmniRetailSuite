using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAppsGateway.Services.Implementation;

public class RetailServiceHealthCheck(IServiceHealthChecker serviceHealthChecker) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var retailServiceUrl = context.Registration.Name;
        var isHealthy = await serviceHealthChecker.CheckServiceHealth(retailServiceUrl);

        return isHealthy ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
    }
}
