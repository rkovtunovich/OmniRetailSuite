namespace Contracts.Dtos.ProductCatalog;

public record BrandDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }
}
