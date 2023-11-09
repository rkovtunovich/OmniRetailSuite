namespace Retail.Core.Entities.ReceiptAggregate;

public class ReceiptItem: BaseEntity
{
    public Guid ReceiptId { get; set; }

    public Receipt Receipt { get; set; } = null!;

    public int LineNumber { get; set; }

    public Guid CatalogItemId { get; set; }

    public CatalogItem CatalogItem { get; set; } = null!;

    public double Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
