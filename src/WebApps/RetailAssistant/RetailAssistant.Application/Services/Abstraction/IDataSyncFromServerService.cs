namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataSyncFromServerService<TModel> where TModel : EntityModelBase, new()
{
    Task SyncAsync(CancellationToken stoppingToken);
}
