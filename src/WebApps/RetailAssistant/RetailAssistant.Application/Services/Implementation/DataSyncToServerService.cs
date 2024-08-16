﻿using Infrastructure.DataManagement.IndexedDb;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncToServerService<TModel> : IDataSyncToServerService<TModel>, IDisposable where TModel : EntityModelBase, new()
{
    private readonly IApplicationStateService _applicationStateService;
    private readonly ILogger<DataSyncToServerService<TModel>> _logger;
    private readonly IRetailDataService<TModel> _retailService;
    private readonly IDbDataService<TModel> _dbDataService;
    private readonly IMapper _mapper;

    private Timer? _toServerSyncTimer;

    public DataSyncToServerService(
        IApplicationStateService applicationStateService,
        IDbDataService<TModel> dbDataService,
        IRetailDataService<TModel> retailService,
        ILogger<DataSyncToServerService<TModel>> logger,
        IMapper mapper)
    {
        _applicationStateService = applicationStateService;
        _retailService = retailService;
        _dbDataService = dbDataService;
        _logger = logger;

        Initialize();
        _mapper = mapper;
    }

    private void Initialize()
    {
        _toServerSyncTimer = new Timer(async _ => await SyncAsync(CancellationToken.None), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
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
    }
}
