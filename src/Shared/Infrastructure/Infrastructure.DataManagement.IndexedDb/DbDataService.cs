using Microsoft.JSInterop;

namespace Infrastructure.DataManagement.IndexedDb;

public class DbDataService<TRecord> : IDbDataService<TRecord>
{
    private readonly Lazy<Task<IJSObjectReference>> _dbInteropModuleTask;

    private readonly ILogger<DbManager> _logger = null!;

    public DbDataService(IJSRuntime jsRuntime, ILogger<DbManager> logger)
    {
        _dbInteropModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Infrastructure.DataManagement.IndexedDb/indexed-db.js").AsTask());
        _logger = logger;
    }

    public async Task AddRecordAsync(string dbName, string storeName, TRecord item)
    {
        try
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;

            await dbInteropModule.InvokeVoidAsync("addRecord", dbName, storeName, item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding item to store '{storeName}' in database '{dbName}'.");
            throw;
        }
    }

    public async Task<IEnumerable<TRecord>> GetAllRecordsAsync(string dbName, string storeName)
    {
        try
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;

            var items = await dbInteropModule.InvokeAsync<IEnumerable<TRecord>>("getAllRecords", dbName, storeName);

            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting all items from store '{storeName}' in database '{dbName}'.");
            throw;
        }
    }

    public async Task<TRecord?> GetRecordAsync(string dbName, string storeName, string key)
    {
        try
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;

            var item = await dbInteropModule.InvokeAsync<TRecord?>("getRecord", dbName, storeName, key);

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting item by key '{key}' from store '{storeName}' in database '{dbName}'.");
            throw;
        }
    }

    public async Task UpdateRecordAsync(string dbName, string storeName, TRecord item)
    {
        try
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;

            await dbInteropModule.InvokeVoidAsync("updateRecord", dbName, storeName, item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating item in store '{storeName}' in database '{dbName}'.");
            throw;
        }
    }

    public async Task DeleteRecordAsync(string dbName, string storeName, string key)
    {
        try
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;

            await dbInteropModule.InvokeVoidAsync("deleteRecord", dbName, storeName, key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting item by key '{key}' from store '{storeName}' in database '{dbName}'.");
            throw;
        }
    }

    public async Task ClearStoreAsync(string dbName, string storeName)
    {
        try
        {
            var dbInteropModule = await _dbInteropModuleTask.Value;

            await dbInteropModule.InvokeVoidAsync("clearStore", dbName, storeName);

            _logger.LogInformation($"Store '{storeName}' in database '{dbName}' has been cleared.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error clearing store '{storeName}' in database '{dbName}'.");
            throw;
        }
    }
}
