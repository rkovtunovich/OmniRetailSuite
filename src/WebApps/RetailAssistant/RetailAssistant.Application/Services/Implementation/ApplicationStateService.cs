using HealthChecks.UI.Core;
using Microsoft.Extensions.Options;
using RetailAssistant.Core.Models.Settings;

namespace RetailAssistant.Application.Services.Implementation;

public class ApplicationStateService : IApplicationStateService, IDisposable
{
    private readonly IOptions<GatewaySettings> _gatewaySettings;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IDataSerializer _dataSerializer;

    private readonly ILogger<ApplicationStateService> _logger;

    private Timer _timer = null!;

    private ApplicationMode _mode;

    public bool IsOnline => _mode == ApplicationMode.Online;

    public event Action? OnStateChange;

    public ApplicationStateService(IOptions<GatewaySettings> gatewaySettings, IHttpClientFactory httpClientFactory, ILogger<ApplicationStateService> logger, IDataSerializer dataSerializer)
    {
        _gatewaySettings = gatewaySettings;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _dataSerializer = dataSerializer;

        Initialize();
    }

    private void Initialize()
    {
        _mode = ApplicationMode.Offline;

        var interval = TimeSpan.FromSeconds(_gatewaySettings.Value.HealthCheckInterval);
        if (interval <= TimeSpan.Zero)
            throw new Exception("Health check interval must be greater than zero");

        var timeout = TimeSpan.FromSeconds(_gatewaySettings.Value.HealthCheckTimeout);
        if (timeout <= TimeSpan.Zero)
            throw new Exception("Health check timeout must be greater than zero");

        _timer = new Timer(async _ => await UpdateState(), null, TimeSpan.Zero, TimeSpan.FromSeconds(_gatewaySettings.Value.HealthCheckInterval));
    }

    private async Task UpdateState()
    {
        var currentMode = _mode;

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(_gatewaySettings.Value.HealthCheckTimeout);
            var url = new Uri(new Uri(_gatewaySettings.Value.BaseUrl), _gatewaySettings.Value.HealthCheckPath);
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var healthCheck = _dataSerializer.Deserialize<UIHealthReport>(content)
                ?? throw new Exception("Failed to deserialize health check response");

            _mode = healthCheck.Status is UIHealthStatus.Healthy ? ApplicationMode.Online : ApplicationMode.Offline;

            _logger.LogInformation($"Health check success: Service is {(IsOnline ? "Online" : "Offline")}");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Health check request timed out");
            _mode = ApplicationMode.Offline;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "HTTP request failed during health check");
            _mode = ApplicationMode.Offline;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unexpected error during health check");
            _mode = ApplicationMode.Offline;
        }

        if (currentMode != _mode)
            OnStateChange?.Invoke();
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
