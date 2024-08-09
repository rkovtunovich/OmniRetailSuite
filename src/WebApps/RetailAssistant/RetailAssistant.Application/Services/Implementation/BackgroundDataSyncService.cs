using Microsoft.Extensions.Hosting;

namespace RetailAssistant.Application.Services.Implementation;

public class BackgroundDataSyncService(
    IApplicationStateService applicationStateService,
    ILogger<BackgroundDataSyncService> logger,
    IProductCatalogService<CatalogProductItem> productItemService,
    IProductCatalogService<ProductParent> productParentService) : BackgroundService
{

    private readonly TimeSpan _syncInterval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!applicationStateService.IsOnline)
            {
                logger.LogInformation("Device is offline. Data sync is disabled.");
                await Task.Delay(_syncInterval, stoppingToken);
                continue;
            }

            logger.LogInformation("Starting data sync...");

            try
            {
                await SyncFromServer();
                await SyncToServer();

                logger.LogInformation("Data sync completed. Next sync in 5 minutes.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Data sync failed.");
            }

            await Task.Delay(_syncInterval, stoppingToken);
        }
    }

    private async Task SyncFromServer()
    {
        var productItems = await productItemService.GetAllAsync();
        var productParents = await productParentService.GetAllAsync();
    }

    private async Task SyncToServer()
    {
        // TO DO
        await Task.FromResult(0);
    }
}
