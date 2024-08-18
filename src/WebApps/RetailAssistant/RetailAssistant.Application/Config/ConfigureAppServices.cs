using Blazored.LocalStorage;
using Infrastructure.Common.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RetailAssistant.Application.Mapping.Configuration;
using RetailAssistant.Data;

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

        services.AddScoped<IRetailDataService<Cashier>, RetailService<Cashier, CashierDto>>();
        services.AddScoped<IDataService<Cashier>>((provider) => provider.GetRequiredService<IRetailDataService<Cashier>>());

        services.AddScoped<IRetailDataService<Receipt>, RetailService<Receipt, ReceiptDto>>();
        services.AddScoped<IDataService<Receipt>>((provider) => provider.GetRequiredService<IRetailDataService<Receipt>>());

        services.AddScoped<IProductCatalogDataService<CatalogProductItem>, ProductCatalogService<CatalogProductItem, ProductItemDto>>();
        services.AddScoped<IDataService<CatalogProductItem>>((provider) => provider.GetRequiredService<IProductCatalogDataService<CatalogProductItem>>());

        services.AddScoped<IProductCatalogDataService<ProductParent>, ProductCatalogService<ProductParent, ProductParentDto>>();
        services.AddScoped<IDataService<ProductParent>>((provider) => provider.GetRequiredService<IProductCatalogDataService<ProductParent>>());

        services.AddScoped(typeof(IDataSyncFromServerService<,>), typeof(DataSyncFromServerService<,>));

        services.AddMapping();

        services.AddSingleton<IGuidGenerator, GuidGenerator>();

        return services;
    }

    public static void StartDataSynchronization(this WebAssemblyHost host)
    {
        var services = host.Services;

        // create the instances of the data synchronization services for starting the sync process
        services.GetRequiredService<IDataSyncFromServerService<CatalogProductItem, ProductCatalogDbSchema>>();
        services.GetRequiredService<IDataSyncFromServerService<ProductParent, ProductCatalogDbSchema>>();
        services.GetRequiredService<IDataSyncFromServerService<Store, RetailDbSchema>>();
        services.GetRequiredService<IDataSyncFromServerService<Cashier, RetailDbSchema>>();
    }
}
