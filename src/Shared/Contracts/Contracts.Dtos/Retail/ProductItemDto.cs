namespace Contracts.Dtos.Retail;

public record ProductItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }
    
    public string? CodePrefix { get; init; }
}
