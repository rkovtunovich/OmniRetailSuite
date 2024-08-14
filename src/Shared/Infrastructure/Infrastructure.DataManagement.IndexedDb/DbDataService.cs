using Microsoft.JSInterop;

namespace Infrastructure.DataManagement.IndexedDb;

public class DbDataService<T> : IDbDataService<T>
{
    private readonly Lazy<Task<IJSObjectReference>> _dbInteropModuleTask;

    private readonly ILogger<DbManager> _logger = null!;

    public DbDataService(IJSRuntime jsRuntime, ILogger<DbManager> logger)
    {
        _dbInteropModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Infrastructure.DataManagement.IndexedDb/indexed-db.js").AsTask());
        _logger = logger;
    }

    public async Task AddItemAsync(string dbName, string storeName, T item)
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

    public Task DeleteItemAsync(string dbName, string storeName, string key)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAllItemsAsync(string dbName, string storeName)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetItemAsync(string dbName, string storeName, string key)
    {
        throw new NotImplementedException();
    }

    public Task UpdateItemAsync(string dbName, string storeName, T item)
    {
        throw new NotImplementedException();
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
