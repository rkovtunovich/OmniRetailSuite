namespace Contracts.Dtos.ProductCatalog;

public record ProductTypeDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }
}
