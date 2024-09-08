using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Polly.Registry;
using RetailAssistant.Data;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncFromServerService<TModel, TDbSettings>(
    IApplicationStateService applicationStateService,
    IApplicationRepository<TModel, TDbSettings> applicationRepository,
    IDataService<TModel> dataService,
    ILogger<DataSyncFromServerService<TModel, TDbSettings>> logger,
    IOptions<TDbSettings> options,
    ResiliencePipelineProvider<string> resiliencePipelineProvider,
    AuthenticationStateProvider authenticationStateProvider): 
    DataSyncServiceBase<TModel, TDbSettings>(applicationStateService, applicationRepository, dataService, logger, options, resiliencePipelineProvider, authenticationStateProvider),
    IDataSyncFromServerService<TModel, TDbSettings>
    where TModel : EntityModelBase, new()
    where TDbSettings : DbSchema
{
    protected override async Task PerformSynchronizationAsync(CancellationToken stoppingToken)
    {
        var items = await _dataService.GetAllAsync();

        foreach (var item in items)
            await _applicationRepository.CreateOrUpdateAsync(item);
    }
}
