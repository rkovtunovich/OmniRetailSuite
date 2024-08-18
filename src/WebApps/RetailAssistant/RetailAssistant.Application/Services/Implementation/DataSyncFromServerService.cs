using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.Extensions.Options;
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

    private Timer? _fromServerSyncTimer;

    public DataSyncFromServerService(
        IApplicationStateService applicationStateService,
        IApplicationRepository<TModel, TDbSettings> applicationRepository,
        IDataService<TModel> dataService,
        ILogger<DataSyncFromServerService<TModel, TDbSettings>> logger,
        IOptions<TDbSettings> options)
    {
        _applicationStateService = applicationStateService;
        _dataService = dataService;
        _applicationRepository = applicationRepository;
        _logger = logger;
        _options = options;

        Initialize();
    }

    private void Initialize()
    {
        var interval =  TimeSpan.FromMinutes(_options.Value.SynchronizationInterval);
        _fromServerSyncTimer = new Timer(async _ => await SyncAsync(CancellationToken.None), null, TimeSpan.Zero, interval);

        _logger.LogInformation($"Data sync timer initialized for {typeof(TModel).Name} with interval {interval}.");
    }

    public async Task SyncAsync(CancellationToken stoppingToken)
    {
        if (!_applicationStateService.IsOnline)
        {
            _logger.LogInformation("Device is offline. Data sync is disabled.");
            return;
        }

        _logger.LogInformation("Starting loading data from server...");

        try
        {
            var dbName = _options.Value.Name;

            var productItems = await _dataService.GetAllAsync();

            foreach (var productItem in productItems)
            {
                await _applicationRepository.CreateOrUpdateAsync(productItem);
            }

            _logger.LogInformation("Data loaded from server.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data loading from server failed.");
        }
    }

    public void Dispose()
    {
        _fromServerSyncTimer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
