namespace Contracts.Dtos.ProductCatalog;

public record ItemTypeDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }
}
