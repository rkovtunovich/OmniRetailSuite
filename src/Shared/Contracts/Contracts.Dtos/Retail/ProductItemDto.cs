namespace Contracts.Dtos.Retail;

public record ProductItemDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }
    
    public string? CodePrefix { get; init; }
}
