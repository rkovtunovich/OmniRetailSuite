namespace Retail.Core.Entities.ReceiptAggregate;

public class Receipt: BaseEntity, ICodedEntity
{
    public int CodeNumber { get; set; }

    public string? CodePrefix { get; set; }

    public DateTimeOffset Date { get; set; }

    public Guid StoreId { get; set; }

    public Store Store { get; set; } = null!;

    public Guid CashierId { get; set; }

    public Cashier Cashier { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];
}
