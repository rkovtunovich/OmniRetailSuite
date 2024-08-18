using Infrastructure.DataManagement.IndexedDb;
using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.Extensions.Options;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncToServerService<TModel, TDbSettings> : IDataSyncToServerService<TModel>, IDisposable 
    where TModel : EntityModelBase, new()
    where TDbSettings : DbSchema
{
    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSyncToServerService<TModel, TDbSettings>> _logger;
    private readonly IRetailDataService<TModel> _retailService;
    private readonly IDbDataService<TModel> _dbDataService;
    private readonly IOptions<TDbSettings> _options;

    private Timer? _toServerSyncTimer;

    public DataSyncToServerService(
        IApplicationStateService applicationStateService,
        IDbDataService<TModel> dbDataService,
        IRetailDataService<TModel> retailService,
        ILogger<DataSyncToServerService<TModel, TDbSettings>> logger,
        IOptions<TDbSettings> options)
    {
        _applicationStateService = applicationStateService;
        _retailService = retailService;
        _dbDataService = dbDataService;
        _logger = logger;
        _options = options;

        Initialize();
    }

    private void Initialize()
    {
        var interval = TimeSpan.FromMinutes(_options.Value.SynchronizationInterval);
        _toServerSyncTimer = new Timer(async _ => await SyncAsync(CancellationToken.None), null, TimeSpan.Zero, interval);
    }

    public async Task SyncAsync(CancellationToken stoppingToken)
    {
        if (!_applicationStateService.IsOnline)
        {
            _logger.LogInformation("Device is offline. Data sync is disabled.");
            return;
        }

        _logger.LogInformation("Starting uploading data to server...");

        try
        {
            // TODO: Implement the logic to upload data to the server
            await Task.FromResult(0);

            _logger.LogInformation("Data uploaded to server.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data uploading to server failed.");
        }
    }

    public void Dispose()
    {
        _toServerSyncTimer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
