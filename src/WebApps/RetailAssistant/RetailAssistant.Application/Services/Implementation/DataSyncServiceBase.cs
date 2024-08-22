using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using RetailAssistant.Core.Models.Settings;
using RetailAssistant.Data;

namespace RetailAssistant.Application.Services.Implementation;

public abstract class DataSyncServiceBase<TModel, TDbSettings> : 
    IDisposable
    where TModel : EntityModelBase, new()
    where TDbSettings : DbSchema
{
    protected readonly IApplicationStateService _applicationStateService;
    protected readonly ILogger<DataSyncServiceBase<TModel, TDbSettings>> _logger;
    protected readonly IDataService<TModel> _dataService;
    protected readonly IApplicationRepository<TModel, TDbSettings> _applicationRepository;
    protected readonly IOptions<TDbSettings> _options;
    protected readonly ResiliencePipeline _resiliencePipeline;

    private Timer? _fromServerSyncTimer;
    private bool _isTimerInitialized = false;
    private readonly object _timerLock = new();

    public DataSyncServiceBase(
        IApplicationStateService applicationStateService,
        IApplicationRepository<TModel, TDbSettings> applicationRepository,
        IDataService<TModel> dataService,
        ILogger<DataSyncServiceBase<TModel, TDbSettings>> logger,
        IOptions<TDbSettings> options,
        ResiliencePipelineProvider<string> resiliencePipelineProvider)
    {
        _applicationStateService = applicationStateService;
        _dataService = dataService;
        _applicationRepository = applicationRepository;
        _logger = logger;
        _options = options;
        _resiliencePipeline = resiliencePipelineProvider.GetPipeline(RetryPolicySettings.Key);
    }

    protected abstract Task PerformSynchronizationAsync(CancellationToken stoppingToken);

    public async Task SyncAsync(CancellationToken stoppingToken)
    {
        EnsureTimerInitialized();

        if (!_applicationStateService.IsOnline)
        {
            _logger.LogInformation("Device is offline. Data sync is disabled.");
            return;
        }

        _logger.LogInformation($"Starting sync {typeof(TModel).Name} data.");

        try
        {
            await _resiliencePipeline.ExecuteAsync(async (stoppingToken) =>
            {
                await PerformSynchronizationAsync(stoppingToken);              
            });

            _logger.LogInformation($"Data sync completed for {typeof(TModel).Name}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Data sync failed for {typeof(TModel).Name}.");
        }
    }

    private void EnsureTimerInitialized()
    {
        if (_isTimerInitialized) 
            return;

        lock (_timerLock)
        {
            if (_isTimerInitialized) 
                return;

            var interval = TimeSpan.FromMinutes(_options.Value.SynchronizationInterval);
            _fromServerSyncTimer = new Timer(async _ => await SyncAsync(CancellationToken.None), null, TimeSpan.Zero, interval);

            _isTimerInitialized = true;

            _logger.LogInformation($"Data sync timer initialized for {typeof(TModel).Name} with interval {interval}.");
        }
    }

    public void Dispose()
    {
        _fromServerSyncTimer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
