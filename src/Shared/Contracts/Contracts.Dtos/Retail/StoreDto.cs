namespace Contracts.Dtos.Retail;

public record StoreDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }

    public string? Address { get; init; }

    public List<CashierDto>? Cashiers { get; init; }
}
