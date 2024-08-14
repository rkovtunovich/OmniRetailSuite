namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataSyncToServerService<TModel> where TModel : EntityModelBase, new()
{
    Task SyncAsync(CancellationToken stoppingToken);
}
