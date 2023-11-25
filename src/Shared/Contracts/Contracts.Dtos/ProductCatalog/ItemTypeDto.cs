namespace Contracts.Dtos.ProductCatalog;

public record ItemTypeDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }
}
