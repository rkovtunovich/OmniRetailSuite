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
                Name = AppDatabase.ProductCatalog.ToString(),
                Version = 1,
                ObjectStores = GetProductCatalogStoreDefinitions()
            };
            await host.PrepareDatabase(productCatalogDbSchema);

            var retailDbSchema = new DbSchema
            {
                Name = AppDatabase.Retail.ToString(),
                Version = 1,
                ObjectStores = GetRetailStoreDefinitions()
            };
            await host.PrepareDatabase(retailDbSchema);

            logger.LogInformation("IndexedDb configured.");

            // create the instances of the data synchronization services for starting the sync process
            services.GetRequiredService<IDataSyncFromServerService<CatalogProductItem>>();
            services.GetRequiredService<IDataSyncFromServerService<ProductParent>>();
            services.GetRequiredService<IDataSyncFromServerService<Store>>();
            services.GetRequiredService<IDataSyncFromServerService<Cashier>>();
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
                KeyPath = nameof(ProductBrand.Id).ToLower()
            },
            new StoreDefinition
            {
                Name = nameof(ProductParent),
                KeyPath = nameof(ProductParent.Id).ToLower()
            },
            new StoreDefinition
            {
                Name = nameof(ProductType),
                KeyPath = nameof(ProductType.Id).ToLower()
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
                KeyPath = nameof(Store.Id).ToLower()
            },
            new StoreDefinition
            {
                Name = nameof(Cashier),
                KeyPath = nameof(Cashier.Id).ToLower()
            },
            new StoreDefinition
            {
                Name = nameof(Receipt),
                KeyPath = nameof(Receipt.Id).ToLower()
            }
        ];
    }

    public static void AddIndexedDb(this IServiceCollection services)
    {
        services.AddDataManagement();
        services.AddScoped<IDataSyncFromServerService<CatalogProductItem>, DataSyncFromServerService<CatalogProductItem>>();
        services.AddScoped<IDataSyncFromServerService<ProductParent>, DataSyncFromServerService<ProductParent>>();
        services.AddScoped<IDataSyncFromServerService<Store>, DataSyncFromServerService<Store>>();
        services.AddScoped<IDataSyncFromServerService<Cashier>, DataSyncFromServerService<Cashier>>();

        services.AddScoped<IDbDataService<CatalogProductItem>, DbDataService<CatalogProductItem>>();
        services.AddScoped<IDbDataService<ProductParent>, DbDataService<ProductParent>>();
        services.AddScoped<IDbDataService<Store>, DbDataService<Store>>();
        services.AddScoped<IDbDataService<Cashier>, DbDataService<Cashier>>();
    }
}
