namespace RetailAssistant.Core.Models.Retail;

public class Receipt : EntityModelBase
{
    public DateTimeOffset Date { get; set; }

    public Guid StoreId { get; set; }

    public Store Store { get; set; } = null!;

    public Guid CashierId { get; set; }

    public Cashier Cashier { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];
}
