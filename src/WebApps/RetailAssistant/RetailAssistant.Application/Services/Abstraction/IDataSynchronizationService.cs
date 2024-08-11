namespace RetailAssistant.Application.Services.Abstraction;

public interface IDataSynchronizationService
{
    Task SyncFromServerAsync(CancellationToken stoppingToken);

    Task SyncToServerAsync(CancellationToken stoppingToken);
}
