namespace Contracts.Dtos.ProductCatalog;

public record ItemParentDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public ItemParentDto? Parent { get; init; }

    public IEnumerable<ItemParentDto>? Children { get; init; }
}
