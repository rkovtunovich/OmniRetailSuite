namespace ProductCatalog.Data.Queries;

public record ItemParentTree
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ItemParentTree? Parent { get; init; }

    public List<Guid> ChildIds { get; init; } = [];
}
