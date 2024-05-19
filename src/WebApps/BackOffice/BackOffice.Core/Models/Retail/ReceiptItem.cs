namespace BackOffice.Core.Models.Retail;

public class ReceiptItem
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }

    public Receipt Receipt { get; set; } = null!;

    public int LineNumber { get; set; }

    public Guid ProductItemId { get; set; }

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}

