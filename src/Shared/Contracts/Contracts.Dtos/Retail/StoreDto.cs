namespace Contracts.Dtos.Retail;

public class StoreDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; } = null!;

    public string Address { get; init; } = null!;

    public List<CashierDto> Cashiers { get; init; } = null!;
}
