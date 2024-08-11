namespace RetailAssistant.Application.Services.Implementation;

public class DataSynchronizationService : IDataSynchronizationService, IDisposable
{
    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSynchronizationService> _logger;
    private readonly IProductCatalogService<CatalogProductItem> _productItemService;
    private readonly IProductCatalogService<ProductParent> _productParentService;

    private Timer? _fromServerSyncTimer;
    private Timer? _toServerSyncTimer;

    public DataSynchronizationService(
        IApplicationStateService applicationStateService,
        ILogger<DataSynchronizationService> logger,
        IProductCatalogService<CatalogProductItem> productItemService,
        IProductCatalogService<ProductParent> productParentService)
    {
        _applicationStateService = applicationStateService;
        _logger = logger;
        _productItemService = productItemService;
        _productParentService = productParentService;

        Initialize();
    }

    private void Initialize()
    {
        _fromServerSyncTimer = new Timer(async _ => await SyncFromServerAsync(CancellationToken.None), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
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
            var productItems = await _productItemService.GetAllAsync();
            var productParents = await _productParentService.GetAllAsync();

            // TO DO

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
