using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;

namespace RetailAssistant.Data;

public class RetailDbSchema : DbSchema
{
    public RetailDbSchema()
    {
        Name = AppDatabase.Retail.ToString();
        Version = 1;
        SynchronizationInterval = 1;
        ObjectStores = GetRetailStoreDefinitions();
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
}
