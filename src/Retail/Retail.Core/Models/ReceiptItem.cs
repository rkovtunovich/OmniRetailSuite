namespace Retail.Core.Models;

public class ReceiptItem
{
    public Receipt Receipt { get; set; } = null!;

    public int Number { get; set; }

    public CatalogItem CatalogItem { get; set; } = null!;

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
