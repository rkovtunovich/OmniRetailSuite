namespace RetailAssistant.Application.Services.Abstraction;

public interface ILocalConfigService
{
    Task<RetailAssistantAppConfig> GetConfigAsync();

    Task SaveConfigAsync(RetailAssistantAppConfig config);
}
