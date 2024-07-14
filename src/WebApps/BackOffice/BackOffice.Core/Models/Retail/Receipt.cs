namespace BackOffice.Core.Models.Retail;

public class Receipt : EntityModelBase
{
    public DateTimeOffset Date { get; set; }

    public Store Store { get; set; } = null!;

    public Cashier Cashier { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = [];
}
