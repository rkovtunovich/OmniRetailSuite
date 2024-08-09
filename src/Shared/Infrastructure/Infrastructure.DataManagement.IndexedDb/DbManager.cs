using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.JSInterop;

namespace Infrastructure.DataManagement.IndexedDb;

public class DbManager : IDbManager<DbSchema>, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime = null!;

    private readonly Lazy<Task<IJSObjectReference>> _dbInteropModuleTask;

    public DbManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _dbInteropModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Infrastructure.DataManagement.IndexedDb/indexed-db.js").AsTask());
    }

    public async Task EnsureDatabaseExists(DbSchema? dbSettings)
    {
        ArgumentNullException.ThrowIfNull(dbSettings);

        var dbInteropModule = await _dbInteropModuleTask.Value;

        if (await dbInteropModule.InvokeAsync<bool>("isDbCreated", dbSettings.Name))
            return;

        //if(await _jsRuntime.InvokeAsync<bool>("isDbCreated", dbSettings.Name))
        //    return;

        await dbInteropModule.InvokeVoidAsync("initializeDb", dbSettings);
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
