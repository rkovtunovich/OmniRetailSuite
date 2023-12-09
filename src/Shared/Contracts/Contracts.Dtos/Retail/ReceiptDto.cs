namespace Contracts.Dtos.Retail;

public record ReceiptDto
{
    public Guid Id { get; init; }

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; } = null!;

    public DateTimeOffset Date { get; init; }

    public Guid CashierId { get; init; }

    public Guid StoreId { get; init; }

    public decimal TotalPrice { get; init; }

    public List<ReceiptItemDto> ReceiptItems { get; init; } = null!;
}
