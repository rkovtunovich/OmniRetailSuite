using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;

namespace RetailAssistant.Data;

public class ProductCatalogDbSchema : DbSchema
{
    public ProductCatalogDbSchema()
    {
        Name = AppDatabase.ProductCatalog.ToString();
        Version = 1;
        SynchronizationInterval = 1;
        ObjectStores = GetProductCatalogStoreDefinitions();
    }

    private static List<StoreDefinition> GetProductCatalogStoreDefinitions()
    {
        return
        [
            new StoreDefinition
            {
                Name = nameof(CatalogProductItem),
                KeyPath = nameof(CatalogProductItem.Id).ToCamelCase(),
                Indexes = [ 
                    new() 
                    { 
                        Name = nameof(CatalogProductItem.ParentId).ToCamelCase(), 
                        KeyPath = nameof(CatalogProductItem.ParentId).ToCamelCase(),
                        Unique = false 
                    }
                ]
            },
            new StoreDefinition
            {
                Name = nameof(ProductBrand),
                KeyPath = nameof(ProductBrand.Id).ToCamelCase()
            },
            new StoreDefinition
            {
                Name = nameof(ProductParent),
                KeyPath = nameof(ProductParent.Id).ToCamelCase()
            },
            new StoreDefinition
            {
                Name = nameof(ProductType),
                KeyPath = nameof(ProductType.Id).ToCamelCase()
            },
        ];
    }
}
