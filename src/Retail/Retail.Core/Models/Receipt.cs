namespace Retail.Core.Models;

public class Receipt
{
    public int Id { get; init; }

    public string Number { get; set; } = null!;

    public DateTimeOffset Date { get; set; }
  
    public Cashier Cashier { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public List<ReceiptItem> ReceiptItems { get; set; } = new();
}
