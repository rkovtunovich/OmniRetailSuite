namespace WebAppsGateway.Services.Abstraction;

public interface IServiceHealthChecker
{
    Task<bool> CheckServiceHealth(string url);
}
