namespace Contracts.Dtos.ProductCatalog;

public record BrandDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }
}
