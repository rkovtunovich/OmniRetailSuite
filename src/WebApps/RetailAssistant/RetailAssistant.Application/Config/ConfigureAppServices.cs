using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using RetailAssistant.Application.Mapping.Configuration;

namespace RetailAssistant.Application.Config;

public static class ConfigureAppServices
{
    public static IServiceCollection AddRetailAssistantAppServices(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
        services.AddScoped<ILocalConfigService, LocalConfigService>();

        services.AddScoped<IRetailService<Store>, RetailService<Store, StoreDto>>();

        services.AddMapping();

        return services;
    }
}
