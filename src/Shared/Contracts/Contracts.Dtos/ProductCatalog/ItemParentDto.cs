namespace Contracts.Dtos.ProductCatalog;

public record ItemParentDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }

    public Guid? ParentId { get; set; }

    public IEnumerable<ItemParentDto>? Children { get; set; }
}
