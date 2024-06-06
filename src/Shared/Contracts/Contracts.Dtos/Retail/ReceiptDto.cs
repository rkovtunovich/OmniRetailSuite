namespace Contracts.Dtos.Retail;

public record ReceiptDto : EntityDtoBase
{
    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; } = null!;

    public DateTimeOffset Date { get; init; }

    public CashierDto Cashier { get; init; }

    public StoreDto Store { get; init; }

    public decimal TotalPrice { get; init; }

    public List<ReceiptItemDto>? ReceiptItems { get; init; }
}
