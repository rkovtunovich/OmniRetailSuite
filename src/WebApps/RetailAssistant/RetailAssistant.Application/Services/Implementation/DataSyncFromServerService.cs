using Infrastructure.DataManagement.IndexedDb;
using RetailAssistant.Data;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncFromServerService<TModel> : IDataSyncFromServerService<TModel>, IDisposable where TModel : EntityModelBase, new()
{
    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSyncFromServerService<TModel>> _logger;
    private readonly IDataService<TModel> _dataService;
    private readonly IDbDataService<TModel> _dbDataService;
    private readonly IMapper _mapper;

    private Timer? _fromServerSyncTimer;

    public DataSyncFromServerService(
        IApplicationStateService applicationStateService,
        IDbDataService<TModel> dbDataService,
        IDataService<TModel> dataService,
        ILogger<DataSyncFromServerService<TModel>> logger,
        IMapper mapper)
    {
        _applicationStateService = applicationStateService;
        _dataService = dataService;
        _dbDataService = dbDataService;
        _logger = logger;
        _mapper = mapper;

        Initialize();
    }

    private void Initialize()
    {
        var interval = TimeSpan.FromMinutes(1);
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
            var dbName = _mapper.Map<AppDatabase>(new TModel()).ToString();

            var productItems = await _dataService.GetAllAsync();
            await _dbDataService.ClearStoreAsync(dbName, typeof(TModel).Name);
            foreach (var productItem in productItems)
            {
                await _dbDataService.AddRecordAsync(dbName, typeof(TModel).Name, productItem);
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
