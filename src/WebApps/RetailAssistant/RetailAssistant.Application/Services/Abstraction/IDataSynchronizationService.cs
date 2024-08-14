namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataSynchronizationService<TModel> where TModel : EntityModelBase, new()
{
    Task SyncFromServerAsync(CancellationToken stoppingToken);

    Task SyncToServerAsync(CancellationToken stoppingToken);
}
