using Infrastructure.DataManagement.IndexedDb;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSynchronizationService<TModel> : IDataSynchronizationService<TModel>, IDisposable where TModel : EntityModelBase, new()
{
    private const string ProductCatalogDbName = "productCatalog";
    private const string RetailDbName = "retail";

    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSynchronizationService<TModel>> _logger;
    private readonly IProductCatalogService<TModel> _productCatalogService;
    private readonly IDbDataService<TModel> _dbDataService;

    private Timer? _fromServerSyncTimer;
    private Timer? _toServerSyncTimer;

    public DataSynchronizationService(
        IApplicationStateService applicationStateService,
        IDbDataService<TModel> dbDataService,
        IProductCatalogService<TModel> productCatalogService,
        ILogger<DataSynchronizationService<TModel>> logger)
    {
        _applicationStateService = applicationStateService;
        _productCatalogService = productCatalogService;
        _dbDataService = dbDataService;
        _logger = logger;

        Initialize();
    }

    private void Initialize()
    {
        _fromServerSyncTimer = new Timer(async _ => await SyncFromServerAsync(CancellationToken.None), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        _toServerSyncTimer = new Timer(async _ => await SyncToServerAsync(CancellationToken.None), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
    }

    public async Task SyncFromServerAsync(CancellationToken stoppingToken)
    {
        if (!_applicationStateService.IsOnline)
        {
            _logger.LogInformation("Device is offline. Data sync is disabled.");
            return;
        }

        _logger.LogInformation("Starting loading data from server...");

        try
        {
            var productItems = await _productCatalogService.GetAllAsync();
            await _dbDataService.ClearStoreAsync(ProductCatalogDbName, typeof(TModel).Name);
            foreach (var productItem in productItems)
            {
                await _dbDataService.AddItemAsync(ProductCatalogDbName, typeof(TModel).Name, productItem);
            }

            _logger.LogInformation("Data loaded from server.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data loading from server failed.");
        }
    }

    public async Task SyncToServerAsync(CancellationToken stoppingToken)
    {
        if (!_applicationStateService.IsOnline)
        {
            _logger.LogInformation("Device is offline. Data sync is disabled.");
            return;
        }

        _logger.LogInformation("Starting data sync to server...");

        try
        {
            // TO DO
            await Task.FromResult(0);
            _logger.LogInformation("Data sync to server completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data sync to server failed.");
        }
    }

    public void Dispose()
    {
        _fromServerSyncTimer?.Dispose();
        _toServerSyncTimer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
