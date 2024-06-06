namespace Contracts.Dtos.Retail;

public record RetailProductItemDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }
    
    public string? CodePrefix { get; init; }
}
