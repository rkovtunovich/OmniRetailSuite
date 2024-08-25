namespace Infrastructure.DataManagement.IndexedDb.Configuration.Settings;

public record StoreDefinition
{
    public string Name { get; init; } = null!;

    public string KeyPath { get; init; } = null!;

    public bool AutoIncrement { get; init; }

    public IList<IndexDefinition>? Indexes { get; init; }
}
