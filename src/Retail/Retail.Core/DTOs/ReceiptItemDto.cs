using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.DTOs;

public record ReceiptItemDto
{
    public Guid Id { get; init; }

    public int LineNumber { get; init; }

    public ReceiptDto Receipt { get; init; } = null!;

    public CatalogItemDto CatalogItem { get; init; }

    public string ProductName { get; init; } = null!;

    public double Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal TotalPrice { get; init; }

    public static ReceiptItemDto FromReceiptItem(ReceiptItem receiptItem)
    {
        return new ReceiptItemDto
        {
            Id = receiptItem.Id,
            LineNumber = receiptItem.LineNumber,
            Receipt = ReceiptDto.FromReceipt(receiptItem.Receipt),
            CatalogItem = CatalogItemDto.FromCatalogItem(receiptItem.CatalogItem),
            Quantity = receiptItem.Quantity,
            UnitPrice = receiptItem.UnitPrice,
            TotalPrice = receiptItem.TotalPrice
        };
    }

    public static List<ReceiptItemDto> FromReceiptItems(List<ReceiptItem> receiptItems)
    {
        return receiptItems.Select(FromReceiptItem).ToList();
    }

    public ReceiptItem ToReceiptItem()
    {
        return new ReceiptItem
        {
            Id = Id,
            LineNumber = LineNumber,
            ReceiptId = Receipt.Id,
            Receipt = Receipt.ToReceipt(),
            CatalogItemId = CatalogItem.Id,
            CatalogItem = CatalogItem.ToCatalogItem(),
            Quantity = Quantity,
            UnitPrice = UnitPrice,
            TotalPrice = TotalPrice
        };
    }
}
