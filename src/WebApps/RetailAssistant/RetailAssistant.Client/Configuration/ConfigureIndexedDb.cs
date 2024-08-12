using Infrastructure.DataManagement.Abstraction;
using Infrastructure.DataManagement.IndexedDb;
using Infrastructure.DataManagement.IndexedDb.Configuration;
using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Infrastructure.DataManagement.IndexedDb.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RetailAssistant.Core.Models.ProductCatalog;

namespace RetailAssistant.Client.Configuration;

public static class ConfigureIndexedDb
{
    public static async Task ConfigureIndexedDbAsync(this WebAssemblyHost host)
    {
        var services = host.Services;

        var logger = services.GetRequiredService<ILogger<IDbManager<DbSchema>>>();

        logger.LogInformation("Configuring IndexedDb...");

        try
        {
            var productCatalogDbSchema = new DbSchema
            {
                Name = "productCatalog",
                Version = 1,
                ObjectStores = GetProductCatalogStoreDefinitions()
            };
            await host.PrepareDatabase(productCatalogDbSchema);

            var retailDbSchema = new DbSchema
            {
                Name = "retail",
                Version = 1,
                ObjectStores = GetRetailStoreDefinitions()
            };

            logger.LogInformation("IndexedDb configured.");

            // create the instance of the data synchronization service for starting the sync process
            var syncService = services.GetRequiredService<IDataSynchronizationService>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred configuring IndexedDb.");
        }
    }

    private static List<StoreDefinition> GetProductCatalogStoreDefinitions()
    {
        return
        [
            new StoreDefinition
            {
                Name = nameof(CatalogProductItem),
                KeyPath = nameof(CatalogProductItem.Id).ToLower()
            },
            new StoreDefinition
            {
                Name = nameof(ProductBrand),
                KeyPath = nameof(ProductBrand.Id)
            },
            new StoreDefinition
            {
                Name = nameof(ProductParent),
                KeyPath = nameof(ProductParent.Id)
            },
            new StoreDefinition
            {
                Name = nameof(ProductType),
                KeyPath = nameof(ProductType.Id)
            },
        ];
    }

    private static List<StoreDefinition> GetRetailStoreDefinitions()
    {
        return
        [
            new StoreDefinition
            {
                Name = nameof(Store),
                KeyPath = nameof(Store.Id)
            },
            new StoreDefinition
            {
                Name = nameof(Cashier),
                KeyPath = nameof(Cashier.Id)
            },
            new StoreDefinition
            {
                Name = nameof(Receipt),
                KeyPath = nameof(Receipt.Id)
            }
        ];
    }

    public static void AddIndexedDb(this IServiceCollection services)
    {
        services.AddDataManagement();
        services.AddScoped<IDataSynchronizationService, DataSynchronizationService>();

        services.AddScoped<IDbDataService<CatalogProductItem>, DbDataService<CatalogProductItem>>();
    }
}
