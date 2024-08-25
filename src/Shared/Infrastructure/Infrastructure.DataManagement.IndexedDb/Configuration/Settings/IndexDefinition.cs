namespace Infrastructure.DataManagement.IndexedDb.Configuration.Settings;

public record IndexDefinition
{
    public string Name { get; init; } = null!;

    public string KeyPath { get; init; } = null!;

    public bool Unique { get; init; }
}
