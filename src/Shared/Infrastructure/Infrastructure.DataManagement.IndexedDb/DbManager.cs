﻿using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.JSInterop;

namespace Infrastructure.DataManagement.IndexedDb;

public class DbManager : IDbManager<DbSchema>, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime = null!;

    private readonly Lazy<Task<IJSObjectReference>> _dbInteropModuleTask;

    private readonly ILogger<DbManager> _logger = null!;

    public DbManager(IJSRuntime jsRuntime, ILogger<DbManager> logger)
    {
        _jsRuntime = jsRuntime;
        _dbInteropModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Infrastructure.DataManagement.IndexedDb/indexed-db.js").AsTask());
        _logger = logger;
    }

    public async Task EnsureDatabaseExists(DbSchema? dbSettings)
    {
        ArgumentNullException.ThrowIfNull(dbSettings);

        var dbInteropModule = await _dbInteropModuleTask.Value;

        var result = await dbInteropModule.InvokeAsync<bool>("initializeDb", dbSettings);

        if(result)
            _logger.LogInformation($"Database '{dbSettings.Name}' has been initialized and is ready for use.");
        else
            _logger.LogError($"Error initializing database '{dbSettings.Name}'.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_dbInteropModuleTask.IsValueCreated)
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;
            await dbInteropModule.DisposeAsync();
        }
    }
}
