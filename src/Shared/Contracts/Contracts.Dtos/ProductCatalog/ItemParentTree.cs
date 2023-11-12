namespace Contracts.Dtos.ProductCatalog;

public record ItemParentTree
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ItemParentDto? Parent { get; init; }

    public List<int> ChildIds { get; init; } = new();
}
