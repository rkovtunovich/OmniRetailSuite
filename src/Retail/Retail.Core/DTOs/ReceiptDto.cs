using Retail.Core.Entities.ReceiptAggregate;

namespace Retail.Core.DTOs;

public record ReceiptDto
{
    public Guid Id { get; init; }

    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; } = null!;

    public DateTimeOffset Date { get; init; }

    public CashierDto Cashier { get; init; } = null!;

    public decimal TotalPrice { get; init; }

    public List<ReceiptItemDto> ReceiptItems { get; init; } = null!;

    public static ReceiptDto FromReceipt(Receipt receipt)
    {
        return new ReceiptDto
        {
            Id = receipt.Id,
            Date = receipt.Date,
            CodeNumber = receipt.CodeNumber,
            CodePrefix = receipt.CodePrefix,
            Cashier = CashierDto.FromCashier(receipt.Cashier),
            ReceiptItems = ReceiptItemDto.FromReceiptItems(receipt.ReceiptItems)
        };
    }

    public static List<ReceiptDto> FromReceipts(List<Receipt> receipts)
    {
        return receipts.Select(FromReceipt).ToList();
    }

    public Receipt ToReceipt()
    {
        return new Receipt
        {
            Id = Id,
            Date = Date,
            CodeNumber = CodeNumber,
            CodePrefix = CodePrefix,
            CashierId = Cashier.Id,
            Cashier = Cashier.ToCashier(),
            TotalPrice = TotalPrice,
            ReceiptItems = ReceiptItems.Select(x => x.ToReceiptItem()).ToList()
        };
    }
}
