using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using RetailAssistant.Core.Models.Settings;
using RetailAssistant.Data;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncFromServerService<TModel, TDbSettings> : IDataSyncFromServerService<TModel, TDbSettings>, IDisposable
    where TModel : EntityModelBase, new()
    where TDbSettings : DbSchema
{
    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSyncFromServerService<TModel, TDbSettings>> _logger;
    private readonly IDataService<TModel> _dataService;
    private readonly IApplicationRepository<TModel, TDbSettings> _applicationRepository;
    private readonly IOptions<TDbSettings> _options;
    private readonly ResiliencePipeline _resiliencePipeline;

    private Timer? _fromServerSyncTimer;
    private bool _isTimerInitialized = false;
    private readonly object _timerLock = new();

    public DataSyncFromServerService(
        IApplicationStateService applicationStateService,
        IApplicationRepository<TModel, TDbSettings> applicationRepository,
        IDataService<TModel> dataService,
        ILogger<DataSyncFromServerService<TModel, TDbSettings>> logger,
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

    public async Task SyncAsync(CancellationToken stoppingToken)
    {
        EnsureTimerInitialized();

        if (!_applicationStateService.IsOnline)
        {
            _logger.LogInformation("Device is offline. Data sync is disabled.");
            return;
        }

        _logger.LogInformation($"Starting loading {typeof(TModel).Name} data from server.");

        try
        {
            await _resiliencePipeline.ExecuteAsync(async (stoppingToken) =>
            {
                var dbName = _options.Value.Name;

                var productItems = await _dataService.GetAllAsync();

                foreach (var productItem in productItems)               
                    await _applicationRepository.CreateOrUpdateAsync(productItem);              
            });

            _logger.LogInformation($"Data loading from server completed for {typeof(TModel).Name}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Data loading from server failed for {typeof(TModel).Name}.");
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
