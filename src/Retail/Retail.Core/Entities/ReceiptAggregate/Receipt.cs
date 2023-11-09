namespace Retail.Core.Entities.ReceiptAggregate;

public class Receipt: BaseEntity, INumberedEntity
{
    public string Number { get; set; } = null!;

    public DateTimeOffset Date { get; set; }

    public Guid CashierId { get; set; }
    
    public Cashier Cashier { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];
}
