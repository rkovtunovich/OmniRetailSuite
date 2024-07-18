using HealthChecks.UI.Core;
using Infrastructure.Serialization.Abstraction;

namespace WebAppsGateway.Services.Implementation;

public class ServiceHealthChecker(IHttpClientFactory httpClientFactory,
                                  IDataSerializer dataSerializer,
                                  ILogger<ServiceHealthChecker> logger) : IServiceHealthChecker
{
    public async Task<bool> CheckServiceHealth(string serviceName)
    {
        var url = $"https://{serviceName}.omni-rs.com/_health";

        try
        {
            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var response = await client.GetAsync(url);
            if (response is null)
            {
                logger.LogWarning($"Service is not available {serviceName}");
                return false;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var healthCheck = dataSerializer.Deserialize<UIHealthReport>(content);

                if (healthCheck is null)
                {
                    logger.LogWarning($"Service is not available {serviceName}");
                    return false;
                }

                logger.LogInformation($"Service is available {serviceName}");

                return healthCheck.Status == UIHealthStatus.Healthy;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Service is not available {serviceName}");

            return false;
        }    
    }
}
