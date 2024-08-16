using Blazored.LocalStorage;
using Infrastructure.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using RetailAssistant.Application.Mapping.Configuration;

namespace RetailAssistant.Application.Config;

public static class ConfigureAppServices
{
    public static IServiceCollection AddRetailAssistantAppServices(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
        services.AddScoped<ILocalConfigService, LocalConfigService>();

        services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        services.AddScoped<IRetailDataService<Store>, RetailService<Store, StoreDto>>();
        services.AddScoped<IDataService<Store>>((provider) => provider.GetRequiredService<IRetailDataService<Store>>());

        services.AddScoped<IRetailDataService<Receipt>, RetailService<Receipt, ReceiptDto>>();
        services.AddScoped<IDataService<Receipt>>((provider) => provider.GetRequiredService<IRetailDataService<Receipt>>());

        services.AddScoped<IProductCatalogDataService<CatalogProductItem>, ProductCatalogService<CatalogProductItem, ProductItemDto>>();
        services.AddScoped<IDataService<CatalogProductItem>>((provider) => provider.GetRequiredService<IProductCatalogDataService<CatalogProductItem>>());

        services.AddScoped<IProductCatalogDataService<ProductParent>, ProductCatalogService<ProductParent, ProductParentDto>>();
        services.AddScoped<IDataService<ProductParent>>((provider) => provider.GetRequiredService<IProductCatalogDataService<ProductParent>>());

        services.AddMapping();

        services.AddSingleton<IGuidGenerator, GuidGenerator>();

        return services;
    }
}
