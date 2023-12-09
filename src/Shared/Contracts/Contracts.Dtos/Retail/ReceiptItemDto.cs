namespace Contracts.Dtos.Retail;

public record ReceiptItemDto
{
    public Guid Id { get; init; }

    public int LineNumber { get; init; }

    public Guid ReceiptId { get; init; }

    public ProductItemDto ProductItem { get; init; }

    public double Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal TotalPrice { get; init; }
}
