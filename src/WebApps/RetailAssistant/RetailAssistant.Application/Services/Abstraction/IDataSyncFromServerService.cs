namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataSyncFromServerService<TModel, TDbSettings> where TModel : EntityModelBase, new()
{
    Task SyncAsync(CancellationToken stoppingToken);
}
