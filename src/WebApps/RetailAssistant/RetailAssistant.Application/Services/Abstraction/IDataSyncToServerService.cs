namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataSyncToServerService<TModel, TDbSettings> where TModel : EntityModelBase, new()
{
    Task SyncAsync(CancellationToken stoppingToken);
}
