using Blazored.LocalStorage;

namespace RetailAssistant.Application.Services.Implementation;

public class LocalConfigService : ILocalConfigService
{
    private readonly ILocalStorageService _localStorageService;

    private RetailAssistantAppConfig? _config;

    public LocalConfigService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<RetailAssistantAppConfig> GetConfigAsync()
    {
        if (_config is not null)        
            return _config;

        _config = await _localStorageService.GetItemAsync<RetailAssistantAppConfig>(nameof(RetailAssistantAppConfig));

        return _config ?? new();
    }

    public async Task SaveConfigAsync(RetailAssistantAppConfig config)
    {
        _config = config;

        await _localStorageService.SetItemAsync(nameof(RetailAssistantAppConfig), config);
    }
}
