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

        services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        services.AddScoped<IRetailService<Store>, RetailService<Store, StoreDto>>();
        services.AddScoped<IRetailService<Receipt>, RetailService<Receipt, ReceiptDto>>();
        services.AddScoped<IProductCatalogService<CatalogProductItem>, ProductCatalogService<CatalogProductItem, Contracts.Dtos.ProductCatalog.ProductItemDto>>();
        services.AddScoped<IProductCatalogService<ProductParent>, ProductCatalogService<ProductParent, ProductParentDto>>();

        services.AddMapping();

        return services;
    }
}
