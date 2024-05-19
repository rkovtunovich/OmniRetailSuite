namespace BackOffice.Core.Models.Retail;

public class Receipt : EntityModelBase
{
    public DateTimeOffset Date { get; set; }

    public Guid StoreId { get; set; }

    public Guid CashierId { get; set; }

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];
}
