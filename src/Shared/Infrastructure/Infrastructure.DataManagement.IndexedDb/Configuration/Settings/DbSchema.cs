namespace Infrastructure.DataManagement.IndexedDb.Configuration.Settings;

public class DbSchema
{
    public string Name { get; set; } = null!;

    public int Version { get; set; }

    public int SynchronizationInterval { get; set; }

    public List<StoreDefinition> ObjectStores { get; set; } = null!;
}
