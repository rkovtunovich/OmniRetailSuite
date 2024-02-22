namespace Contracts.Dtos.ProductCatalog;

public record ProductParentDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }

    public Guid? ParentId { get; set; }

    public IEnumerable<ProductParentDto>? Children { get; set; }
}
