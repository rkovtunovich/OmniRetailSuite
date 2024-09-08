using Helpers.StringsHelper;
using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Polly.Registry;
using RetailAssistant.Data;

namespace RetailAssistant.Application.Services.Implementation;

public class DataSyncToServerService<TModel, TDbSettings>(
    IApplicationStateService applicationStateService,
    IApplicationRepository<TModel, TDbSettings> applicationRepository,
    IDataService<TModel> dataService,
    ILogger<DataSyncToServerService<TModel, TDbSettings>> logger,
    IOptions<TDbSettings> options,
    ResiliencePipelineProvider<string> resiliencePipelineProvider,
    AuthenticationStateProvider authenticationStateProvider) :
    DataSyncServiceBase<TModel, TDbSettings>(applicationStateService, applicationRepository, dataService, logger, options, resiliencePipelineProvider, authenticationStateProvider),
    IDataSyncToServerService<TModel, TDbSettings>
    where TModel : EntityModelBase, new()
    where TDbSettings : DbSchema
{
    private static readonly string _uploadedAtPropertyName = nameof(EntityModelBase.UploadedAt).ToCamelCase();
    private static readonly string _valueForUploadedAt = DateTimeOffset.MinValue.ToString("yyyy-MM-ddTHH:mm:sszzz");

    protected override async Task PerformSynchronizationAsync(CancellationToken stoppingToken)
    {
        var items = await _applicationRepository.GetAllByPropertyAsync(_uploadedAtPropertyName, _valueForUploadedAt);

        var uploadedCount = 0;
        var allItemsCount = items.Count();

        foreach (var item in items)
        {
            var createdItem = await _dataService.CreateAsync(item);

            if (createdItem != null)
            {
                createdItem.UploadedAt = DateTimeOffset.Now;
                await _applicationRepository.UpdateAsync(createdItem);

                uploadedCount++;
            }          
        }

        _logger.LogInformation($"{typeof(TModel).Name}: uploaded {uploadedCount} from {allItemsCount} items to the server.");
    }
}
