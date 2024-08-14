using Infrastructure.DataManagement.IndexedDb;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncFromServerService<TModel> : IDataSyncFromServerService<TModel>, IDisposable where TModel : EntityModelBase, new()
{
    private const string ProductCatalogDbName = "productCatalog";

    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSyncFromServerService<TModel>> _logger;
    private readonly IProductCatalogService<TModel> _productCatalogService;
    private readonly IDbDataService<TModel> _dbDataService;

    private Timer? _fromServerSyncTimer;

    public DataSyncFromServerService(
        IApplicationStateService applicationStateService,
        IDbDataService<TModel> dbDataService,
        IProductCatalogService<TModel> productCatalogService,
        ILogger<DataSyncFromServerService<TModel>> logger)
    {
        _applicationStateService = applicationStateService;
        _productCatalogService = productCatalogService;
        _dbDataService = dbDataService;
        _logger = logger;

        Initialize();
    }

    private void Initialize()
    {
        _fromServerSyncTimer = new Timer(async _ => await SyncAsync(CancellationToken.None), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
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

    public void Dispose()
    {
        _fromServerSyncTimer?.Dispose();

        GC.SuppressFinalize(this);
    }
}
