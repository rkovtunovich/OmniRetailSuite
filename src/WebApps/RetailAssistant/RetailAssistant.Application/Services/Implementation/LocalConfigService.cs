using Blazored.LocalStorage;

namespace RetailAssistant.Application.Services.Implementation;

public class LocalConfigService(ILocalStorageService localStorageService) : ILocalConfigService
{
    private RetailAssistantAppConfig? _config;

    public async Task<RetailAssistantAppConfig> GetConfigAsync()
    {
        if (_config is not null)        
            return _config;

        _config = await localStorageService.GetItemAsync<RetailAssistantAppConfig>(nameof(RetailAssistantAppConfig));

        return _config ?? new();
    }

    public async Task SaveConfigAsync(RetailAssistantAppConfig config)
    {
        _config = config;

        await localStorageService.SetItemAsync(nameof(RetailAssistantAppConfig), config);
    }
}
