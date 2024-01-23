namespace Contracts.Dtos.Retail;

public record CashierDto : EntityDtoBase
{
    public string Name { get; init; } = null!;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; } = null!;
}
